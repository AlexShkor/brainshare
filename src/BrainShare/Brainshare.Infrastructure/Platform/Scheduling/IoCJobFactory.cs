using System;
using Quartz;
using Quartz.Spi;
using StructureMap;

namespace Brainshare.Infrastructure.Platform.Scheduling
{
    public class IoCJobFactory : IJobFactory
    {
        private readonly IContainer _container;

        public IoCJobFactory(IContainer container)
        {
            _container = container;
        }

        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            IJobDetail jobDetail = bundle.JobDetail;
            Type jobType = jobDetail.JobType;

            // Return job that is registrated in container
            return _container.GetInstance(jobType) as IJob;
        }

        public void ReturnJob(IJob job)
        {
            //TODO Not sure what this method supposed to do
        }
    }
}
