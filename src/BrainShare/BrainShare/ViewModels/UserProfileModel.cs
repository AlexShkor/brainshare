using BrainShare.Documents;

namespace BrainShare.ViewModels
{
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public AddressData Address { get; set; }
        public string Email { get; set; }
        public string Info { get; set; }

        public bool IsMe { get; set; }
        public bool IsCurrentUserSubscribed { get; set; }
        public int SummaryVotes { get; set; }

        public bool CanIncrease { get; set; }
        public bool CanDecrease { get; set; }

        public UserProfileModel(User user, User me)
        {
            Id = user.Id;
            Name = user.FullName;
            IsMe = user.Id == me.Id;
            Avatar = user.AvatarUrl ?? Constants.DefaultAvatarUrl;
            Address = user.Address;
            Email = user.Email;
            Info = user.Info;
            IsCurrentUserSubscribed = me.IsSubscribed(Id);
        }
    }
}