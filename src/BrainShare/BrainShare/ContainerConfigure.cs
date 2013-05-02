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
            container.Configure(c=>
                {
                    c.For<MongoDocumentsDatabase>().Singleton().Use(database);
                    c.For<IAuthentication>().Transient().Use<CustomAuthentication>();
                    c.For<MailService>().Transient().Use<MailService>();
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