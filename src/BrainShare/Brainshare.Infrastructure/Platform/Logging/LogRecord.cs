using System;
using Brainshare.Infrastructure.Platform.Mongo;
using MongoDB.Bson;

namespace Brainshare.Infrastructure.Platform.Logging
{
    /// <summary>
    /// Represent one physical log record
    /// </summary>
    public class LogRecord
    {
        /// <summary>
        /// Command (always one)
        /// </summary>
        public CommandRecord Command { get; set; }

        /// <summary>
        /// Events rised for command
        /// </summary>
        public EventRecordCollection Events { get; set; }

        private Int32 _errors;

        public int Errors
        {
            get { return _errors; }
        }

        /// <summary>
        /// From BSON
        /// </summary>
        public static LogRecord FromBson(BsonDocument doc)
        {
            var record = new LogRecord();
            record.Command = CommandRecord.FromBson(doc);
            record.Events = EventRecordCollection.FromBson(doc.GetBsonArray("Events"));
            record._errors = record.Command.Handlers.Errors + record.Events.Errors;
            return record;
        }  

    }
}
