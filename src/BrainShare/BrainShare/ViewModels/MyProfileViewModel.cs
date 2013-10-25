using System.Web;
using BrainShare.Documents;

namespace BrainShare.ViewModels
{
    public class MyProfileViewModel
    {
        public MyProfileViewModel()
        {

        }

        public MyProfileViewModel(User user)
        {
            Name = user.FullName;
            AvatarUrl = user.AvatarUrl ?? Constants.DefaultAvatarUrl;
        }

        public string Name { get; set; }
        public string AvatarUrl { get; set; }

        public HttpPostedFileBase UploadedFile { get; set; }
        public int Id { get; set; }
    }
}