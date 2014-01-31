using System.IO;
using BrainShare.Authentication;
using BrainShare.Domain.Multitenancy;
using BrainShare.Infrastructure.Mongo;
using BrainShare.Services;
using Brainshare.Infrastructure.Authentication;
using Brainshare.Infrastructure.Infrastructure.Multitenancy;
using Brainshare.Infrastructure.Services;
using Brainshare.Infrastructure.Settings;
using StructureMap;
using System.Web.Routing;

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
                    c.For<ICommonUserService>().Use<CommonUserService>();
                    c.For<OzIsbnService>().Singleton().Use<OzIsbnService>();

                    //c.Scan(scan => scan.AssembliesFromPath(Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/"), "Tenants")));
                    c.Scan(scan => scan.AddAllTypesOf<ApplicationTenant>());
                });
        }
    }
}