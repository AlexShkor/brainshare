using Brainshare.Infrastructure.Authentication;
using BrainShare.Infrastructure.Mongo;
using Brainshare.Infrastructure.Services;
using Brainshare.Infrastructure.Settings;
using BrainShare.Services;
using StructureMap;

namespace Brainshare.Infrastructure
{
    public class ContainerConfigure
    {
        public static void Configure(IContainer container)
        {
            var settings = SettingsMapper.Map<Settings.Settings>();

            var database = new MongoDocumentsDatabase(settings.MongoConnectionString);
            database.EnsureIndexes();
            container.Configure(c =>
                {
                    c.For<MongoDocumentsDatabase>().Singleton().Use(database);
                    c.For<Settings.Settings>().Singleton().Use(settings);
                    c.For<IAuthentication>().Transient().Use<CustomAuthentication>();
                    c.For<ICommonUserService>().Use<CommonUserService>();
                    c.For<OzIsbnService>().Singleton().Use<OzIsbnService>();
                });
        }
    }
}