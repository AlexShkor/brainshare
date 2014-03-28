using Brainshare.Infrastructure.Documents;
using Brainshare.Infrastructure.Documents.Data;
using BrainShare.Utils.Utilities;
using Brainshare.Infrastructure.Infrastructure;

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
        public bool IsNonAuthorizedRequest { get; set; }

        public string Status { get; set; }

        public UserProfileModel(User user, User me, int userActivityTimeoutInMinutes,bool isNonAuthorizedRequest = false)
        {
            if (isNonAuthorizedRequest)
            {
                IsNonAuthorizedRequest = true;
            }
            else
            {
                IsMe = user.Id == me.Id;
                IsCurrentUserSubscribed = me.IsSubscribed(Id);
            }

            Id = user.Id;
            Name = user.FullName;
            Avatar = user.AvatarUrl ?? Constants.DefaultAvatarUrl;
            Address = user.Address;
            Email = user.Email;
            Info = user.Info;          
            Status = StringUtility.GetUserStatus(user.LastVisited, userActivityTimeoutInMinutes);
        }
    }
}