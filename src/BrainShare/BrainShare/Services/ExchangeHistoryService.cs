using System.Collections.Generic;
using BrainShare.Documents;
using BrainShare.Mongo;
using MongoDB.Driver;

namespace BrainShare.Services
{
    public class ExchangeHistoryService: DocumentsService<ExchangeHistory>
    {
        public ExchangeHistoryService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoCollection<ExchangeHistory> Items
        {
            get { return Database.ExchangeHistory; }
        }

        public void SaveExchange(string initiatorId, ExchangeEntry firstUser, ExchangeEntry secondUser)
        {
            Save(new ExchangeHistory
                     {
                         Id = GenerateId(),
                         Entries = new List<ExchangeEntry>{firstUser,secondUser},
                         Initiator = initiatorId,
                         Present = false
                     });
        }
    }
}