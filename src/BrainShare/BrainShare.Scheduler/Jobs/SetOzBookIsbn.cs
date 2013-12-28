using System;
using Quartz;

namespace BrainShare.Scheduler.Jobs
{
    public class SetOzBookIsbn : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("JOB IN WORK");
        }
    }
}
