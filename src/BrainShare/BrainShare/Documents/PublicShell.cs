using System;
using BrainShare.Documents.Data;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Documents
{
    public class ShellUser
    {
        [BsonId]
        public string Id { get; set; }
        public ShellAddressData ShellAddressData { get; set; }
        public string Name { get; set; }   
        public DateTime Created { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}