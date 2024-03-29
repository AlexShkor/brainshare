using System;
using System.Collections.Generic;
using BrainShare.Domain.Documents.Data;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Domain.Documents
{
    [BsonIgnoreExtraElements]
    public class ExchangeHistory
    {
        [BsonId]
        public string Id { get; set; }

        public string Initiator { get; set; }

        public List<ExchangeEntry> Entries { get; set; }

        public DateTime Date { get; set; }

        public ExchangeHistory()
        {
            Entries = new List<ExchangeEntry>();
        }

        public ExchangeEntry GetInitiatorEntry()
        {
            return Entries.Find(x => x.User.UserId == Initiator);
        }
    }
}