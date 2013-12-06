using System;
using System.Collections.Generic;
using System.Linq;
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
        public List<Follower> Followers { get; set; }

        public ShellUser()
        {
            Followers = new List<Follower>();
        }

        public void SetFollower(User follower)
        {
            if (Followers.Any(f => f.Id == follower.Id))
            {
                return;
            }

            Followers.Add(new Follower
            {
                Id = follower.Id,
                AvatarUrl = follower.AvatarUrl,
                FullName = follower.FullName,
            });
        }

        public void RemoveFollower(string userId)
        {
            Followers.RemoveAll(e => e.Id == userId);
        }
    }
}