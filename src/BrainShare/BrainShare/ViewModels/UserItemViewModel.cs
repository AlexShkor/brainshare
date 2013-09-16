using BrainShare.Documents;

namespace BrainShare.Controllers
{
    public class UserItemViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string City { get; set; }

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
    }
}