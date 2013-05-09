using BrainShare.Authentication;
using BrainShare.Mongo;
using BrainShare.Services;
using StructureMap;

namespace BrainShare
{
    public class ContainerConfigure
    {
        public static void Configure(IContainer container)
        {
            var database = new MongoDocumentsDatabase("mongodb://admin:1@dbh61.mongolab.com:27617/brainshare");
            container.Configure(c =>
                {
                    c.For<MongoDocumentsDatabase>().Singleton().Use(database);
                    c.For<IAuthentication>().Transient().Use<CustomAuthentication>();
                });


        }
    }
}