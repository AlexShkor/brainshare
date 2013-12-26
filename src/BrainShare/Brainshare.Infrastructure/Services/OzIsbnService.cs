using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace Brainshare.Infrastructure.Services
{
    public class OzIsbnService
    {
        private readonly BooksService _booksService;
        private readonly Settings.Settings _settings;
        private readonly ConnectionFactory _connFactory;

        public OzIsbnService(BooksService books, Settings.Settings settings)
        {
            _booksService = books;
            _connFactory = new ConnectionFactory
            {
                Uri = _settings.RabbitMQUrl,
                Password = _settings.RabbitMQPassword,
                UserName = _settings.RabbitMQUser,
            };
        }

        public void UpdateIsbn(string ozBookId)
        {
            Send(ozBookId);
        }

        private  void Send(string ozBookId)
        {
            // Open up a connection and a channel (a connection may have many channels)
            using (var conn = _connFactory.CreateConnection())
            using (var channel = conn.CreateModel()) // Note, don't share channels between threads
            {
                // the data put on the queue must be a byte array
                var data = Encoding.UTF8.GetBytes(ozBookId);

                // ensure that the queue exists before we publish to it
                channel.QueueDeclare(_settings.RabbitMQRequestIsbnQueuName, false, false, false, null);

                // publish to the "default exchange", with the queue name as the routing key
                channel.BasicPublish("", _settings.RabbitMQRequestIsbnQueuName, null, data);
            }
        }

        private string Receive()
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
                    var text = Encoding.UTF8.GetString(message.Body);


                    // ack the message, ie. confirm that we have processed it
                    // otherwise it will be requeued a bit later
                    subscription.Ack(message);
                }
            }
        }
        }
    }

