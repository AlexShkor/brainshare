using System.Web.Mvc;
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
            var settings = SettingsMapper.Map<Settings>();
            var database = new MongoDocumentsDatabase(settings.MongoConnectionString);
            database.EnsureIndexes();
            container.Configure(c =>
                {
                    c.For<MongoDocumentsDatabase>().Singleton().Use(database);
                    c.For<Settings>().Singleton().Use(settings);
                    c.For<IAuthentication>().Transient().Use<CustomAuthentication>();
                });
        }
    }
}