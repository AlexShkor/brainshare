namespace Brainshare.Infrastructure.Settings
{
    public class Settings
    {
        [SettingsProperty("FacebookAppId")]
        public string FacebookAppId { get; set; }

        [SettingsProperty("FacebookSecretKey")]
        public string FacebookSecretKey { get; set; }

        [SettingsProperty("VkAppId")]
        public string VkAppId { get; set; }

        [SettingsProperty("VkSecretKey")]
        public string VkSecretKey { get; set; }

        [SettingsProperty("mongo_connection_string")]
        public string MongoConnectionString { get; set; }

        [SettingsProperty("ActivityTimeoutInMinutes")]
        public string ActivityTimeoutInMinutes { get; set; }

        [SettingsProperty("cloudinary_url")]
        public string CloudinaryUrl { get; set; }

        [SettingsProperty("AdminDisplayName")]
        public string AdminDisplayName { get; set; }

        [SettingsProperty("adminEmail")]
        public string AdminEmail { get; set; }

        [SettingsProperty("RabbitMQUrl")]
        public string RabbitMQUrl { get; set; }

        [SettingsProperty("RabbitMQUser")]
        public string RabbitMQUser { get; set; }

        [SettingsProperty("RabbitMQPassword")]
        public string RabbitMQPassword { get; set; }

        [SettingsProperty("RabbitMQIsbnQueue")]
        public string RabbitMQIsbnQueue { get; set; }

        [SettingsProperty("mongo.events")]
        public string MongoEventsConnectionString { get; set; }

        [SettingsProperty("mongo.views")]
        public string MongoViewConnectionString { get; set; }

        [SettingsProperty("mongo.logs")]
        public string MongoLogsConnectionString { get; set; }

        [SettingsProperty("mongo.admin")]
        public string MongoAdminConnectionString { get; set; }
    }
}