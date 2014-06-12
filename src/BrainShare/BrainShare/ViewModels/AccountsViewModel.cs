using BrainShare.Domain.Documents;

namespace BrainShare.ViewModels
{
    public class AccountsViewModel
    {
        public string VkId { get; set; }

        public string FbId { get; set; }

        public AccountsViewModel(User user)
        {
            VkId = user.VkId;
            FbId = user.FacebookId;
        }
    }
}