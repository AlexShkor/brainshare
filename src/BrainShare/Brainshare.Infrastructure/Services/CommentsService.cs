using BrainShare.Domain.Documents;
using BrainShare.Infrastructure.Mongo;
using MongoDB.Driver;

namespace Brainshare.Infrastructure.Services
{
    public class CommentsService: DocumentsService<CommentsDocument>
    {
        public CommentsService(MongoDocumentsDatabase database)
            : base(database)
        {
        }

        protected override MongoCollection<CommentsDocument> Items
        {
            get { return Database.Comments; }
        }
    }
}