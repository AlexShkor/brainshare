using System;
using System.Collections.Generic;
using System.Linq;
using BrainShare.Controllers;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Documents
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        public string Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Info { get; set; }

        public Dictionary<string, int> Votes { get; set; }

        public string FacebookId { get; set; }
        public string FacebookAccessToken { get; set; }
        
        public AddressData Address { get; set; }

        public string AvatarUrl { get; set; }
       
        public DateTime Registered { get; set; }

        public List<ChangeRequest> Inbox { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        public User()
        {
            Inbox = new List<ChangeRequest>();
            Votes = new Dictionary<string, int>();
            ThreadsWithUnreadMessages = new List<string>();
            Address = new AddressData();
        }

        public void SetVote(string setterId, int value)
        {
            if (value < -1 || value > 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            Votes[setterId] = value;
        }

        public int GetVote(string userId, string setterId )
        {
            return Votes.ContainsKey(setterId) ? Votes[setterId] : 0;
        }

        public bool IsFacebookAccount 
        {
            get { return !string.IsNullOrWhiteSpace(FacebookId);}
        }

        public List<string> ThreadsWithUnreadMessages { get; set; }

        public int GetSummaryVotes()
        {
            return Votes.Values.Sum(x => x);
        }

        public void RemoveInboxItem(string userId, string yourBookId)
        {
            Inbox.RemoveAll(x => x.User.UserId == userId && x.BookId == yourBookId);
        }
    }
}