namespace BrainShare
{
    public class Settings
    {
        [SettingsProperty("FacebookAppId")]
        public string FacebookAppId { get; set; }

        [SettingsProperty("FacebookSecretKey")]
        public string FacebookSecretKey { get; set; }

        [SettingsProperty("mongo_connection_string")]
        public string MongoConnectionString { get; set; }

        //[SettingsProperty("cloudinary_public_id")]
        //public string CloudinaryPublicId { get; set; }

        [SettingsProperty("cloudinary_url")]
        public string CloudinaryUrl { get; set; }

        //[SettingsProperty("adminEmail")]
        //public string AdminEmail { get; set; }

        //[SettingsProperty("smtpServer")]
        //public string SmtpServer { get; set; }

        //[SettingsProperty("smtpPort")]
        //public string SmtpPort { get; set; }

        //[SettingsProperty("smtpUser")]
        //public string SmtpUser { get; set; }

        //[SettingsProperty("smtpPass")]
        //public string SmtpPass { get; set; }
    }
}