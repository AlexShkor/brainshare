using System.Collections.Generic;
using System.Linq;
using BrainShare.Documents;
using BrainShare.Documents.Data;

namespace BrainShare.ViewModels
{
    public class PublishersViewModel
    {
        public  string  PublisherTemplate = "publisherTemplate";
        public  string  PublisherDeletedTemplate = "publisherDeletedTemplate";
        public string UserName { get; set; }
 
        public PublishersViewModel(IEnumerable<BaseUser> publishers,string userName)
        {
            Publishers = new List<PublisherViewModel>();
            UserName = userName;

            foreach (var publisher in publishers)
            {
                Publishers.Add(new PublisherViewModel
                    {
                        AvatarUrl = publisher.AvatarUrl?? Constants.DefaultAvatarUrl,
                        FullName = publisher.FullName,
                        Id = publisher.Id,
                        IsShell = publisher.UserType == "ShellUser",
                        TemplateName = PublisherTemplate
                    });
            }
        }

        public List<PublisherViewModel> Publishers { get; set; }

        public bool PublishersExist{
            get { return Publishers != null && Publishers.Any(); }
        }
    }
}