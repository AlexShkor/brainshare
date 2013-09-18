using System;
using System.Collections.Generic;
using BrainShare.Documents;

namespace BrainShare.Controllers
{
    public class UserProfileModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public bool IsMe { get; set; }
        public int SummaryVotes { get; set; }

        public bool CanIncrease { get; set; }
        public bool CanDecrease { get; set; }

        public UserProfileModel(User user, string myId)
        {
            Id = user.Id;
            Name = user.FullName;
            IsMe = user.Id == myId;
            Avatar = user.AvatarUrl ?? "http://critterapp.pagodabox.com/img/user.jpg";
        }
    }
}