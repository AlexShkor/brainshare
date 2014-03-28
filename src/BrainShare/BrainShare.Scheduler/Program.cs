using System;
using System.Configuration;
using System.Threading.Tasks;
using Brainshare.Infrastructure.Dto;
using Brainshare.Infrastructure.Mongo;
using BrainShare.Utils.Utilities;
using Brainshare.Infrastructure.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace BrainShare.Worker
{
    class Program
    {
        static readonly string RequestQueuName = ConfigurationManager.AppSettings["RabbitMQIsbnQueue"];

        static readonly BooksService _booksService = new BooksService(new MongoDocumentsDatabase(ConfigurationManager.AppSettings["MongoConnectionString"]));
        static readonly WishBooksService _wishBooksService = new WishBooksService(new MongoDocumentsDatabase(ConfigurationManager.AppSettings["MongoConnectionString"]));

        static readonly ConnectionFactory ConnFactory = new ConnectionFactory
            {
                Uri = ConfigurationManager.AppSettings["RabbitMQUrl"],
                Password = ConfigurationManager.AppSettings["RabbitMQPassword"],
                UserName = ConfigurationManager.AppSettings["RabbitMQUser"],
            };

        static void Main(string[] args)
        {
            using (var conn = ConnFactory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                // ensure that the queue exists before we access it
                channel.QueueDeclare(RequestQueuName, false, false, false, null);

                // subscribe to the queue
                var subscription = new Subscription(channel, RequestQueuName);
                while (true)
                {
                    // this will block until a messages has landed in the queue
                    var message = subscription.Next();

                    // deserialize the message body
                    var request = SerializeUtility.Deserialize<OzBookIsbnRequestDto>(message.Body);

                    Save(request);

                    // ack the message, ie. confirm that we have processed it
                    // otherwise it will be requeued a bit later
                    subscription.Ack(message);
                }
            }
        }

        private static void Save(OzBookIsbnRequestDto request)
        {
            Task.Factory.StartNew(() =>
                {
                    string isbn;
                    try
                    {
                       isbn = OzParser.OzParser.GetIsbnByOzBookId(request.Id);

                       if (request.IsWishedBook)
                       {
                           Update(_wishBooksService, request.Id, isbn);
                       }
                       else
                       {
                           Update(_booksService, request.Id, isbn);
                       } 
                    }
                    catch
                    {    
                    }
                });
        }

        static void Update(BaseBooksService service, string id, string isbn)
        {
            var book = service.GetByOzBookId(id);
            if (!book.ISBN.Contains(isbn))
            {
                book.ISBN.Add(isbn);
                service.Save(book);
            }
        }
    }
}
