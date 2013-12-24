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
        public List<string> Followers { get; set; }
        public List<UserNewsInfo> News { get; set; }
        public string Salt { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsOnline { get; set; }
        public DateTime LastVisited { get; set; }

        public virtual string FullName { get; set; }

        public virtual string UserType { get; set; }

        public virtual void SetFollower(string id)
        {
            if (Followers == null)
            {
                Followers = new List<string>();
            }

            if (Followers.Any(f => f == id))
            {
                throw new Exception(string.Format("follower with id = {0} already exists",id));
            }

            Followers.Add(id);
        }

        public virtual void RemoveFollower(string id)
        {
            Followers.RemoveAll(e => e == id);
        }

        public virtual void SetNewsReadStatusTrue(string id)
        {
            News.Single(n => n.Id == id).WasRead = true;
        }
    }
}