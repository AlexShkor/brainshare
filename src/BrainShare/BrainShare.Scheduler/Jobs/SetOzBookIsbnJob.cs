using System;
using System.Diagnostics;
using System.IO;
using Castle.Components.Scheduler;

namespace BrainShare.Scheduler.Jobs
{
    public class SetOzBookIsbnJob : IJob
    {
        public bool Execute(JobExecutionContext context)
        {
            
            // Use our current state.
            Console.WriteLine("In MyJob class");

            //this code segment write data to file.
            FileStream fs1 = new FileStream("D:\\Yourfile.txt", FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter writer = new StreamWriter(fs1);
            writer.Write("Hello Welcome");
            writer.Close();
            //int currentToken = (int)context.JobData.State["Token"];
            //context.Logger.InfoFormat("Current token is: '{0}'.", currentToken);

            // Update our state for next time.
         //   context.JobData.State[Contrib: "Token"] = currentToken + 1;

            // Return true for success!
            return true;
        }
    }
}
