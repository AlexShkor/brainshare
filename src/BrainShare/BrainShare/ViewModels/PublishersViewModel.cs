using System.Collections.Generic;
using System.Linq;
using BrainShare.Documents;
using BrainShare.Documents.Data;

namespace BrainShare.ViewModels
{
    public class PublishersViewModel
    {
        public PublishersViewModel(IEnumerable<BaseUser> publishers )
        {
            Publishers = new List<Publisher>();

            foreach (var publisher in publishers)
            {
                Publishers.Add(new Publisher
                    {
                        AvatarUrl = publisher.AvatarUrl?? Constants.DefaultAvatarUrl,
                        FullName = publisher.FullName,
                        Id = publisher.Id,
                        IsShell = publisher.UserType == "ShellUser"
                    });
            }
        }

        public List<Publisher> Publishers { get; set; }

        public bool PublishersExist{
            get { return Publishers != null && Publishers.Any(); }
        }
    }
}