using BrainShare.Documents;

namespace BrainShare.Controllers
{
    public class UserItemViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public UserItemViewModel(User doc)
        {
            UserId = doc.Id;
            Email = doc.Email;
            UserName = string.Format("{0} {1}", doc.FirstName, doc.LastName);
        }
    }
}