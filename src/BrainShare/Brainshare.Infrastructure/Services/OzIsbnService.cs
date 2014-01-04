using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using BrainShare.Domain.Dto;
using BrainShare.Services;
using BrainShare.Utils.Utilities;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace Brainshare.Infrastructure.Services
{
    public class OzIsbnService
    {
        private bool _isServiceStarted;
        private readonly Settings.Settings _settings;
        private readonly ConnectionFactory _connFactory;

        private readonly BooksService _booksService;
        private readonly WishBooksService _wishBooksService;

        private readonly ConcurrentQueue<OzBookIsbnRequestDto> _booksWithEmptyIsbnQueue;

        public OzIsbnService(Settings.Settings settings, BooksService booksService, WishBooksService wishBooksService)
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
            ownedBooks.Clear();

            var wishedBooks = _wishBooksService.GetOzIdsWithEmptyIsbn().Select(e => new OzBookIsbnRequestDto { Id = e, IsWishedBook = true }).Where(e => e.Id == "103002").ToList();
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

            var conn = _connFactory.CreateConnection();

         //  MonitorQueue(conn);

         //  StartReceiver(conn);
        }

        public void AddItem(string ozBookId, bool isWishedBook)
        {
            _booksWithEmptyIsbnQueue.Enqueue(new OzBookIsbnRequestDto { Id = ozBookId, IsWishedBook = isWishedBook });
        }

        private Task MonitorQueue(IConnection conn)
        {
           return Task.Factory.StartNew(async() =>
            {
                using (var channel = conn.CreateModel())
                {
                    channel.ModelShutdown += ModelShutdownEventHandler;
                    // ensure that the queue exists before we access it
                    channel.QueueDeclare(_settings.RabbitMQRequestIsbnQueuName, false, false, false, null);

                    while (true)
                    {

                        if (_booksWithEmptyIsbnQueue.Count == 0)
                        {
                            await Task.Delay(10000);
                            continue;
                        }

                        var ozBook = new OzBookIsbnRequestDto();
                        if (_booksWithEmptyIsbnQueue.TryDequeue(out ozBook))
                        {
                            // the data put on the queue must be a byte array
                            var data = SerializeUtility.Serialize(ozBook);

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

        private Task StartReceiver(IConnection conn)
        {
            return Task.Factory.StartNew(() =>
            {
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
                        var responce = SerializeUtility.Deserialize<OzBookIsbnResponceDto>(message.Body);

                        try
                        {
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
                        catch (Exception)
                        {
                        }
                    }
                }
            },
            TaskCreationOptions.LongRunning);
        }
   
        private void Update(BaseBooksService service,OzBookIsbnResponceDto dto)
        {
            var book = service.GetByOzBookId(dto.Id);
            if (!book.ISBN.Contains(dto.Isbn))
            {
                book.ISBN.Add(dto.Isbn);
                service.Save(book);
            }          
        }

        private void ModelShutdownEventHandler(RabbitMQ.Client.IModel model, RabbitMQ.Client.ShutdownEventArgs reason)
        {
            var i = 3;
        }
    }
}