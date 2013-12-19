using System.Web.Hosting;

namespace BrainShare.ViewModels
{
    public class BaseEmailModel
    {
        public string BaseAddress
        {
            get
            {
                return "http://localhost:100";// HostingEnvironment.ApplicationHost.GetSiteName();
            }
        }
    }
}