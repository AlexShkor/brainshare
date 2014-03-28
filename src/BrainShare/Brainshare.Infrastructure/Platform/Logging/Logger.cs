using System;
using Brainshare.Infrastructure.Databases;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Brainshare.Infrastructure.Platform.Logging
{
    public class Logger
    {
        private readonly MongoLogsDatabase _db;

        public Logger(MongoLogsDatabase db)
        {
            _db = db;
        }

        public void Error(string message)
        {
            _db.AppLogs.Insert(new LogItem { Message = message });
        }

        public void Error(Exception ex, string message = null)
        {
            _db.AppLogs.Insert(new LogItem { Exception = ex, Message = message});
        }

        public void Info(string message)
        {
            _db.AppLogs.Insert(new LogItem { Message = message, Level = LogLevelEnum.Info });
        }
    }

    public class LogItem
    {
        [BsonId]
        public string Id { get; set; }

        public string Message { get; set; }

        public Exception Exception { get; set; }

        public DateTime DateTime { get; set; }

        public LogLevelEnum Level { get; set; }

        public LogItem()
        {
            Id = ObjectId.GenerateNewId().ToString();
            DateTime = DateTime.UtcNow;
        }
    }

    public enum LogLevelEnum
    {
        Error,
        Info
    }
}