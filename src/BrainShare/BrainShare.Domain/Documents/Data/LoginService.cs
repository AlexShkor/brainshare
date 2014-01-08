namespace BrainShare.Domain.Documents.Data
{
    public class LoginService
    {
        public LoginServiceTypeEnum LoginType { get; set; }
        public string ServiceUserId { get; set; }
        public string AccessToken { get; set; }
    }
}
