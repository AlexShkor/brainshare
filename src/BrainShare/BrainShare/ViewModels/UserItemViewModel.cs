using BrainShare.Documents;
using Brainshare.Infrastructure.Documents;

namespace BrainShare.ViewModels
{
    public class UserItemViewModel
    {
        public string UserId { get; set; }
        public string BookId { get; set; }
        public string UserName { get; set; }
        public string City { get; set; }

        public string UserProfile {get
        {
           return "/profile/view/" + UserId;
        }}

        public UserItemViewModel(UserData data)
        {
            UserId = data.UserId;
            UserName = data.UserName;
            City = data.Address.Locality;
        }

        public UserItemViewModel(User user)
        {
            UserId = user.Id;
            UserName = user.FullName;
            City = user.Address.Locality;
        }

        public UserItemViewModel()
        {
            
        }

        public UserItemViewModel(Book book)
        {
            BookId = book.Id;
            UserId = book.UserData.UserId;
            UserName = book.UserData.UserName;
            City = book.UserData.Address.Locality;
        }
    }
}