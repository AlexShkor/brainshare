using System;
using BrainShare.Scheduler.Jobs;
using Quartz;
using Quartz.Impl;

namespace BrainShare.Scheduler
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create the scheduler factory
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
 
            //Ask the scheduler factory for a scheduler
            IScheduler scheduler = schedulerFactory.GetScheduler();
 
            //Start the scheduler so that it can start executing jobs
            scheduler.Start();
 
            // Create a job of Type WriteToConsoleJob
            IJobDetail job = JobBuilder.Create(typeof(SetOzBookIsbn)).WithIdentity("MyJob", "MyJobGroup").Build();
 
            //Schedule this job to execute every second, a maximum of 10 times
            ITrigger trigger = TriggerBuilder.Create().WithSchedule(SimpleScheduleBuilder.RepeatSecondlyForTotalCount(10)).StartNow().WithIdentity("MyJobTrigger", "MyJobTriggerGroup").Build();
            scheduler.ScheduleJob(job, trigger);
 
            //Wait for a key press. If we don't wait the program exits and the scheduler gets destroyed
            Console.ReadKey();
 
            //A nice way to stop the scheduler, waiting for jobs that are running to finish
            scheduler.Shutdown(true);
        }

    }
}
