namespace BrainShare.Domain.Documents.Data
{
    public class LoginService
    {
        public LoginServiceTypeEnum LoginType { get; set; }
        public string ServiceUserId { get; set; }
        public string ServiceLinkedEmail { get; set; }
        public string AccessToken { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Salt { get; set; }
    }
}
