using Brainshare.Infrastructure.Platform.Domain.Messages;
using Brainshare.Infrastructure.Platform.Mongo;
using MongoDB.Bson;

namespace Brainshare.Infrastructure.Platform.Logging
{
    public class EventRecord
    {
        public BsonDocument EventDocument { get; set; }
        public EventMetadata Metadata { get; set; }
        public EventHandlerRecordCollection Handlers { get; set; }

        public static EventRecord FromBson(BsonDocument doc)
        {
            var eventDocument = doc.GetBsonDocument("Event");
            var record = new EventRecord()
            {
                EventDocument = eventDocument,
                Metadata = eventDocument.GetBsonDocument("Metadata").CreateEventMetadata(),
                Handlers = EventHandlerRecordCollection.FromBson(doc.GetBsonArray("Handlers"))
            };

            return record;
        }  
    }
}