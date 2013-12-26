using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.MessagePatterns;

namespace BrainShare.Worker
{
    class Program
    {
        static readonly string ResponceQueuName = ConfigurationManager.AppSettings["RabbitMQResponceIsbnQueuName"];
        static readonly string RequestQueuName = ConfigurationManager.AppSettings["RabbitMQRequestIsbnQueuName"];

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
                    var text = Encoding.UTF8.GetString(message.Body);

                    SendIsbnResponce(text);

                    // ack the message, ie. confirm that we have processed it
                    // otherwise it will be requeued a bit later
                    subscription.Ack(message);
                }
            }
        }

        private static void SendIsbnResponce(string ozBookId)
        {
            Task.Factory.StartNew(() =>
                {
                    var isbn = OzParser.OzParser.GetIsbnByOzBookId(ozBookId);
                    Send(isbn);
                });
        }

        private static void Send(string responce)
        {
            // Open up a connection and a channel (a connection may have many channels)
            using (var conn = ConnFactory.CreateConnection())
            using (var channel = conn.CreateModel()) // Note, don't share channels between threads
            {
                // the data put on the queue must be a byte array
                var data = Encoding.UTF8.GetBytes(responce);

                // ensure that the queue exists before we publish to it
                channel.QueueDeclare(ResponceQueuName, false, false, false, null);

                // publish to the "default exchange", with the queue name as the routing key
                channel.BasicPublish("", ResponceQueuName, null, data);
            }
        }
    }
}
