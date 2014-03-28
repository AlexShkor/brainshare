using Brainshare.Infrastructure.Documents;
using Brainshare.Infrastructure.Infrastructure;

namespace BrainShare.ViewModels
{
    public class UserViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string Registered { get; set; }

        public int Rating { get; set; }
        public UserViewModel()
        {

        }

        public UserViewModel(User user)
        {
            UserId = user.Id;
            Username = user.FullName;
            Address = user.Address.Locality;
            Avatar = user.AvatarUrl ?? Constants.DefaultAvatarUrl;
            Registered = user.Registered.ToShortDateString();
            Rating = user.GetSummaryVotes();
        }
    }
}