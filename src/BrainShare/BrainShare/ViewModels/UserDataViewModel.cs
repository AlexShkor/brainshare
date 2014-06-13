using BrainShare.Documents;
using Brainshare.Infrastructure.Infrastructure;

namespace BrainShare.ViewModels
{
    public class UserDataViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }

        public UserDataViewModel(UserData user)
        {
            UserId = user.UserId;
            Name = user.UserName;
            Address = user.Address.Locality;
            Avatar = user.AvatarUrl ?? Constants.DefaultAvatarUrl;
        } 
    }
}