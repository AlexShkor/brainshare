using System.Web;
using BrainShare.Domain.Documents;
using BrainShare.Domain.Documents.Data;
using Brainshare.Infrastructure.Infrastructure;

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
            Address = user.Address;
            Info = user.Info;
            SummaryVotes = user.GetSummaryVotes();
        }

        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public AddressData Address { get; set; }
        public string Info { get; set; }
        public int SummaryVotes { get; set; }

        public HttpPostedFileBase UploadedFile { get; set; }
    }
}