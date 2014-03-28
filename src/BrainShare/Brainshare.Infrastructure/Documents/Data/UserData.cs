namespace Brainshare.Infrastructure.Documents.Data
{
    public class UserData
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public AddressData Address { get; set; }
        public string AvatarUrl { get; set; }


        public UserData()
        {
            
        }

        public UserData(User user)
        {
            UserId = user.Id;
            UserName = user.FullName;
            Address = user.Address;
            AvatarUrl = user.AvatarUrl;
        }
    }
}