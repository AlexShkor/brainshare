using System;
using System.Collections.Generic;
using System.Linq;
using BrainShare.Documents.Data;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Documents
{
    public abstract class BaseUser
    {
        [BsonId]
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public List<Follower> Followers { get; set; }
        public string Salt { get; set; }

        public BaseUser()
        {
            Followers = new List<Follower>();
        }

        public virtual void SetFollower(string avatarUrl, string name, string id)
        {
            if (Followers.Any(f => f.Id == id))
            {
                throw new Exception(string.Format("follower with id = {0} already exists",id));
            }

            Followers.Add(new Follower
            {
                Id = id,
                AvatarUrl = avatarUrl,
                FullName = name,
            });
        }

        public virtual void RemoveFollower(string id)
        {
            Followers.RemoveAll(e => e.Id == id);
        }
    }
}