using System;
using System.Collections.Generic;
using System.Linq;
using BrainShare.Domain.Documents.Data;
using Brainshare.Infrastructure.Authentication;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Domain.Documents
{
    [BsonIgnoreExtraElements]
    public class User : BaseUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Info { get; set; }

        public Dictionary<string, int> Votes { get; set; }

        public bool EmailConfirmed { get; set; }
        
        public AddressData Address { get; set; }

        public UserSettings Settings { get; set; }
       
        public DateTime Registered { get; set; }

        public List<ChangeRequest> Inbox { get; set; }

        public List<string> Publishers { get; set; }

        public override string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        public override string UserType { get { return "User"; }  }

        public User()
        {
            Inbox = new List<ChangeRequest>();
            Votes = new Dictionary<string, int>();
            ThreadsWithUnreadMessages = new List<string>();
            Address = new AddressData();
            Publishers = new List<string>();
            Followers = new List<string>();
            News = new List<UserNewsInfo>();
            Settings = new UserSettings();
          //  LoginServices = new List<LoginService>();
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

        public void SetPublisher(CommonUser publisher)
        {
            if (Publishers.Any(p => p == publisher.Id))
            {
                return;
            }

            Publishers.Add(publisher.Id);
        }

        public void RemovePublisher(string userId)
        {
            Publishers.RemoveAll(e => e == userId);
        }

        public bool IsSubscribed(string userId)
        {
            return Publishers.Any(p => p == userId);
        }

        public void AddNews(string id)
        {
            News.Add(new UserNewsInfo { Id = id});
        }
    }
}