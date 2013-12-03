using System;
using System.Collections.Generic;
using BrainShare.ViewModels;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Documents
{
    public class PublicShell
    {
        [BsonId]
        public string Id { get; set; }
        public string CreatorId { get; set; }
        public string FormattedAddress { get; set; }

        public string Route { get; set; }
        public string StreetNumber { get; set; }
        public string Country { get; set; }

        public string Lng { get; set; }
        public string Lat { get; set; }

        public string Name { get; set; }   
        public string LocalPath { get; set; }
        public DateTime Created { get; set; }

        public List<Book> Books { get; set; }
    }
}