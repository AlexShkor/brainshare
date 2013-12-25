using Castle.Windsor;
using Castle.Windsor.Installer;

namespace BrainShare.Scheduler
{
    public class Bootstrap
    {
        public static IWindsorContainer BootstrapContainer()
        {
            return new WindsorContainer()
               .Install(
                        FromAssembly.This()
                //perhaps pass other installers here if needed               
               );
        }
    }
}
