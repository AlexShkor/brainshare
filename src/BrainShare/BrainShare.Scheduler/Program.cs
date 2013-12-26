using System;
using System.Threading.Tasks;
using Castle.Components.Scheduler;
using Castle.Components.Scheduler.JobStores;
using Castle.Core.Logging;

namespace BrainShare.Scheduler
{
    class Program
    {
        public static Task  Run()
        {


            return Task.Factory.StartNew(() =>
                {

                    IJobStore jobStore = new MemoryJobStore();

                    IJobFactory jobFactory =
                        new Castle.Components.Scheduler.WindsorExtension.WindsorJobFactory(
                            Bootstrap.BootstrapContainer().Kernel);
                    IJobRunner jobRunner = new DefaultJobRunner(jobFactory);


                    DefaultScheduler scheduler = new DefaultScheduler(jobStore, jobRunner);
                    scheduler.Logger = new ConsoleLogger();
                    scheduler.Initialize();

                    // Create some initial state information for the job.  (optional)
                    JobData jobData = new JobData();
                    jobData.State["Token"] = 1;

                    // Create a trigger to fire at 2am local time each day.
                    Trigger trigger =
                        PeriodicTrigger.CreateOneShotTrigger(DateTime.Now.Date.ToUniversalTime().AddSeconds(10));

                    // Create a job specification for my job.
                    JobSpec jobSpec = new JobSpec("My job.", "A nightly maintenance job.", "SetOzBookIsbnJob", trigger);

                    // Create a job.  If it already exists in the persistent store then automatically update
                    // its definition to reflect the provided job specification.  This is a good idea when using
                    // a scheduler cluster because the job is guaranteed to be created exactly once and kept up
                    // to date without it ever being accidentally deleted by one instance while another instance
                    // is processing it.
                    scheduler.CreateJob(jobSpec, CreateJobConflictAction.Update);

                    // Start the scheduler.
                    scheduler.Start();
                }) ;
               
        }

        static  void Main(string[] args)
        {

           Task.WaitAll(Run());
           Console.WriteLine("end");
        }
    }
}
