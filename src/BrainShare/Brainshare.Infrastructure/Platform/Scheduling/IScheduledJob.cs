using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;

namespace Brainshare.Infrastructure.Platform.Scheduling
{
    public interface IScheduledJob : IJob
    {
        JobDetailImpl ConfigureJob();

        SimpleTriggerImpl ConfigureTrigger();

        bool IsEnabled { get; }
    }
}
