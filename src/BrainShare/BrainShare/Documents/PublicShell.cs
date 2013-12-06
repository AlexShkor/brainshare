using System;
using System.Collections.Generic;
using System.Linq;
using BrainShare.Documents.Data;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Documents
{
    public class ShellUser:BaseUser
    {
        public ShellAddressData ShellAddressData { get; set; }
        public string Name { get; set; }   
    }
}