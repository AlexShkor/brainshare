﻿using System.Collections.Generic;
using System.Linq;
using BrainShare.Documents;
using BrainShare.Domain.Documents;
using BrainShare.Infrastructure.Utilities;
using BrainShare.Utils.Utilities;
using Brainshare.Infrastructure.Infrastructure;

namespace BrainShare.ViewModels
{
    public class PublishersViewModel
    {
        public  string  PublisherTemplate = "publisherTemplate";
        public  string  PublisherDeletedTemplate = "publisherDeletedTemplate";

        public string UserName { get; set; }
        public bool IsMyFriends { get; set; }


        public PublishersViewModel(IEnumerable<BaseUser> publishers, string userName, bool isMyFriends, int userActivityTimeoutInMinutes)
        {
            Publishers = new List<PublisherViewModel>();
            UserName = userName;
            IsMyFriends = isMyFriends;

            foreach (var publisher in publishers)
            {
                Publishers.Add(new PublisherViewModel
                    {
                        AvatarUrl = publisher.AvatarUrl?? Constants.DefaultAvatarUrl,
                        FullName = publisher.FullName,
                        Id = publisher.Id,
                        IsShell = publisher.UserType == "ShellUser",
                        TemplateName = PublisherTemplate,
                        Status = StringUtility.GetUserStatus(publisher.LastVisited,userActivityTimeoutInMinutes)
                    });
            }
        }

        public List<PublisherViewModel> Publishers { get; set; }

        public bool PublishersExist{
            get { return Publishers != null && Publishers.Any(); }
        }
    }
}