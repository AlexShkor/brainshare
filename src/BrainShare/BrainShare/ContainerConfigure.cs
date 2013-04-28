using BrainShare.Authentication;
using BrainShare.Mongo;
using StructureMap;

namespace BrainShare
{
    public class ContainerConfigure
    {
        public static void Configure(IContainer container)
        {
            var database = new MongoDocumentsDatabase("mongodb://admin:1@ds061807.mongolab.com:61807/brainshare_prod");
            container.Configure(c=>
                {
                    c.For<MongoDocumentsDatabase>().Singleton().Use(database);
                    c.For<IAuthentication>().Transient().Use<CustomAuthentication>();
                });

            
        }
    }

    public class SessionKeys
    {
        public const string FbCsrfToken = "fb_csrf_token";
        public const string FbAccessToken = "fb_access_token";
        public const string FbExpiresIn = "fb_expires_in";
    }
}