using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Documents
{
    public class Book
    {
        [BsonId]
        public string Id { get; set; }

        public string GoogleBookId { get; set; }
        public List<string> ISBN { get; set; }
        public string Title { get; set; }
        public string SearchInfo { get; set; }
        public string Language { get; set; }
        public int PageCount { get; set; }
        public int? PublishedYear { get; set; }
        public int? PublishedMonth { get; set; }
        public int? PublishedDay { get; set; }
        public string Publisher { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }
        public string Country { get; set; }
        public List<string> Authors { get; set; }
        public List<UserData> Owners { get; set; }
        public List<UserData> Lookers { get; set; }

        public Book()
        {
            ISBN = new List<string>();
            Authors = new List<string>();
            Owners = new List<UserData>();
            Lookers = new List<UserData>();
        }

        public DateTime PublishedDate
        {
            get { return new DateTime(PublishedYear ?? 1, PublishedMonth ?? 1, PublishedDay ?? 1); }
        }

        public bool HasOwner(string userId)
        {
            return Owners.Any(x => x.UserId == userId);
        }

        public void AddOwner(User user)
        {
            Owners.Add(new UserData(user));
        }

        public bool HasLooker(string userId)
        {
            return Lookers.Any(x => x.UserId == userId);
        }

        public void AddLooker(User looker)
        {
            Lookers.Add(new UserData(looker));
        }

        public void RemoveLooker(string id)
        {
            Lookers.RemoveAll(x => x.UserId == id);
        }

        public void RemoveOwner(string id)
        {
            Owners.RemoveAll(x => x.UserId == id);
        }
    }

    public class UserData
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string City { get; set; }

        public UserData()
        {
            
        }

        public UserData(User user)
        {
            UserId = user.Id;
            UserName = user.FullName;
            City = user.City;
        }
    }
}