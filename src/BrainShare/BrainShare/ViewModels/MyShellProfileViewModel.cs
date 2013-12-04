using System.Web;
using BrainShare.Documents;
using BrainShare.Documents.Data;

namespace BrainShare.ViewModels
{
    public class MyShellProfileViewModel
    {
        public MyShellProfileViewModel()
        {

        }

        public MyShellProfileViewModel(ShellUser user)
        {
            Name = user.Name;
            AvatarUrl = Constants.DefaultAvatarUrl;
            Address = user.ShellAddressData;
        }

        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public ShellAddressData Address { get; set; }

        public HttpPostedFileBase UploadedFile { get; set; }
    }
}