using System.Collections.Concurrent;
using System.Threading.Tasks;
using BrainShare.Domain.Dto;
using BrainShare.Utils.Utilities;
using RabbitMQ.Client;

namespace Brainshare.Infrastructure.Services
{
    public class OzIsbnService
    {
        private bool _isServiceStarted;
        private readonly Settings.Settings _settings;
        private readonly ConnectionFactory _connFactory;

        private readonly ConcurrentQueue<OzBookIsbnRequestDto> _booksWithEmptyIsbnQueue;

        public OzIsbnService(Settings.Settings settings)
        {
            _settings = settings;
            _connFactory = new ConnectionFactory
                {
                    Uri = _settings.RabbitMQUrl,
                    UserName = _settings.RabbitMQUser,
                    Password = _settings.RabbitMQPassword
                };

           _booksWithEmptyIsbnQueue = new ConcurrentQueue<OzBookIsbnRequestDto>();
        }


        public void Run()
        {
            if (_isServiceStarted)
            {
                return;
            }

            _isServiceStarted = true;

            MonitorQueue();
        }

        public void AddItem(string ozBookId, bool isWishedBook)
        {
            _booksWithEmptyIsbnQueue.Enqueue(new OzBookIsbnRequestDto { Id = ozBookId, IsWishedBook = isWishedBook });
        }

        private Task MonitorQueue()
        {
           return Task.Factory.StartNew(async() =>
            {
                using(var conn = _connFactory.CreateConnection())
                using (var channel = conn.CreateModel())
                {
                    // ensure that the queue exists before we access it
                    channel.QueueDeclare(_settings.RabbitMQIsbnQueue, false, false, false, null);

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
                            channel.BasicPublish("", _settings.RabbitMQIsbnQueue, null, data);
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
    }
}