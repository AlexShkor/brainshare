using System.Collections.Generic;
using BrainShare.Domain.Documents;
using BrainShare.Domain.Documents.Data;
using BrainShare.Infrastructure.Mongo;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Brainshare.Infrastructure.Services
{
    public class ThreadsService : DocumentsService<Thread>
    {
        public ThreadsService(MongoDocumentsDatabase database) : base(database)
        {
        }

        protected override MongoCollection<Thread> Items
        {
            get { return Database.Threads; }
        }

        public Thread GetFor(string userId, string recipientId)
        {
            return Items.FindOne(
                Query.Or(
                Query.And(Query<Thread>.EQ(x => x.OwnerId,recipientId  ),
                                        Query<Thread>.EQ(x => x.RecipientId, userId)),
                
                Query.And(Query<Thread>.EQ(x => x.OwnerId, userId),
                                        Query<Thread>.EQ(x => x.RecipientId, recipientId))));
        }

        public void PostToThread(string threadId, string userId, string content)
        {
            var message = new Message(userId, content);
            Items.Update(Query<Thread>.EQ(x => x.Id, threadId), Update<Thread>.Push(x => x.Messages, message));
        }

        public IEnumerable<Thread> GetAllForUser(string userId)
        {
            return Items.Find(Query.Or(Query<Thread>.EQ(x => x.OwnerId, userId),
                                        Query<Thread>.EQ(x => x.RecipientId, userId)));
        }
    }
}