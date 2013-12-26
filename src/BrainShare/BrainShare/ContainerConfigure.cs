﻿using BrainShare.Authentication;
using BrainShare.Infrastructure.Mongo;
using BrainShare.Services;
using Brainshare.Infrastructure.Authentication;
using Brainshare.Infrastructure.Settings;
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
                    c.For<ICommonUserService>().Use<CommonUserService>();
                    c.For<OzIsbnService>().Singleton().Use<OzIsbnService>();
                });
        }
    }
}