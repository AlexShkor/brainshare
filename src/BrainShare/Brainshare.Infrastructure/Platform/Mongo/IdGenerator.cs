using MongoDB.Bson;

namespace Brainshare.Infrastructure.Platform.Mongo
{
    public class IdGenerator 
    {
        public string Generate()
        {
            return ObjectId.GenerateNewId().ToString();
        }
    }
}
