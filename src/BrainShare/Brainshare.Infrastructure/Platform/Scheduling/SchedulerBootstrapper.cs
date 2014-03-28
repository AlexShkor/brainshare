using System.Reflection;
using Brainshare.Infrastructure.Platform.StructureMap;
using Microsoft.Practices.ServiceLocation;
using Quartz.Spi;
using StructureMap;

namespace Brainshare.Infrastructure.Platform.Scheduling
{
    public class SchedulerBootstrapper
    {
        public void Configure(IContainer container, Assembly jobsAssembly)
        {
            ServiceLocator.SetLocatorProvider(() => new StructureMapServiceLocator(container));

            container.Configure(config => config.For<IJobFactory>().Use(new IoCJobFactory(container)));

            container.Configure(config => config.Scan(scanner =>
            {
                scanner.Assembly(jobsAssembly);
                scanner.AddAllTypesOf<IScheduledJob>();
            }));
        }
    }
}
