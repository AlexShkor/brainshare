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

        [SettingsProperty("AdminDisplayName")]
        public string AdminDisplayName { get; set; }

        [SettingsProperty("adminEmail")]
        public string AdminEmail { get; set; } 

    }
}