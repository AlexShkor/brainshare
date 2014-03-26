using Microsoft.AspNet.Identity;

namespace BrainShare.Admin.Models
{
    public class ApplicationUser : IUser
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
    }
}