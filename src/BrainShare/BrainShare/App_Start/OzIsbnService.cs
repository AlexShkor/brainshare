using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Threading.Tasks;
using BrainShare.Domain.Dto;
using BrainShare.Services;
using Brainshare.Infrastructure.Services;
using Brainshare.Infrastructure.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace BrainShare
{
    public class OzIsbnService
    {
        private bool _isServiceStarted;
        private readonly Settings _settings;
        private readonly ConnectionFactory _connFactory;

        private readonly BooksService _booksService;
        private readonly WishBooksService _wishBooksService;

        private readonly ConcurrentQueue<OzBookIsbnRequestDto> _booksWithEmptyIsbnQueue;

        public OzIsbnService(Settings settings, BooksService booksService, WishBooksService wishBooksService)
        {
            _settings = settings;
            _booksService = booksService;
            _wishBooksService = wishBooksService;
            _connFactory = new ConnectionFactory
                {
                    Uri = _settings.RabbitMQUrl,
                    UserName = _settings.RabbitMQUser,
                    Password = _settings.RabbitMQPassword
                };

            var ownedBooks = _booksService.GetOzIdsWithEmptyIsbn().Select(e => new OzBookIsbnRequestDto { Id = e, IsWishedBook = false }).ToList();
            var wishedBooks = _wishBooksService.GetOzIdsWithEmptyIsbn().Select(e => new OzBookIsbnRequestDto { Id = e, IsWishedBook = true });
            ownedBooks.AddRange(wishedBooks);

           _booksWithEmptyIsbnQueue = new ConcurrentQueue<OzBookIsbnRequestDto>(ownedBooks);
        }


        public void Run()
        {
            if (_isServiceStarted)
            {
                return;
            }

            _isServiceStarted = true;

            MonitorQueue();

            StartReceiver();
        }

        public void AddItem(string ozBookId, bool isWishedBook)
        {
            _booksWithEmptyIsbnQueue.Enqueue(new OzBookIsbnRequestDto { Id = ozBookId, IsWishedBook = isWishedBook });
        }

        private Task MonitorQueue()
        {
           return Task.Factory.StartNew(async() =>
            {
                using (var conn = _connFactory.CreateConnection())
                using (var channel = conn.CreateModel())
                {
                    // ensure that the queue exists before we access it
                    channel.QueueDeclare(_settings.RabbitMQRequestIsbnQueuName, false, false, false, null);

                    while (true)
                    {
                        if (_booksWithEmptyIsbnQueue.Count == 0)
                        {
                            await Task.Delay(5000);
                        }

                        var ozBook = new OzBookIsbnRequestDto();
                        if (_booksWithEmptyIsbnQueue.TryDequeue(out ozBook))
                        {
                            // the data put on the queue must be a byte array
                            var data = Serialize(ozBook);

                            // publish to the "default exchange", with the queue name as the routing key
                            channel.BasicPublish("", _settings.RabbitMQRequestIsbnQueuName, null, data);
                        }
                        else
                        {
                            await Task.Delay(100);
                        }
                        
                    }
                }
            },
            TaskCreationOptions.LongRunning);
        }

        private Task StartReceiver()
        {
            return Task.Factory.StartNew(() =>
            {
                using (var conn = _connFactory.CreateConnection())
                using (var channel = conn.CreateModel())
                {
                    // ensure that the queue exists before we access it
                    channel.QueueDeclare(_settings.RabbitMQResponceIsbnQueuName, false, false, false, null);

                    // subscribe to the queue
                    var subscription = new Subscription(channel, _settings.RabbitMQResponceIsbnQueuName);
                    while (true)
                    {
                        // this will block until a messages has landed in the queue
                        var message = subscription.Next();

                        // deserialize the message body
                        var responce = Deserialize<OzBookIsbnResponceDto>(message.Body);

                        if (responce.IsWishedBook)
                        {
                            Update(_wishBooksService, responce);
                        }
                        else
                        {
                            Update(_booksService, responce);
                        }

                        // ack the message, ie. confirm that we have processed it
                        // otherwise it will be requeued a bit later
                        subscription.Ack(message);
                    }
                }
            },
            TaskCreationOptions.LongRunning);
        }

        private byte[] Serialize(object objectToSerialize)
        {
            byte[] serializedObject;

            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, objectToSerialize);
                serializedObject = stream.ToArray();
            }

            return serializedObject;
        }

        private T Deserialize<T>(byte[] serializedType)
        {
            T deserializedObject;
            using (var memoryStream = new MemoryStream(serializedType))
            {
                var deserializer = new BinaryFormatter();
                deserializedObject = (T)deserializer.Deserialize(memoryStream);
            }

            return deserializedObject;
        }
   
        private void Update(BaseBooksService service,OzBookIsbnResponceDto dto)
        {
            var book = service.GetById(dto.Id);
            book.ISBN.Add(dto.Isbn);
            service.Save(book);
        }
    }
}