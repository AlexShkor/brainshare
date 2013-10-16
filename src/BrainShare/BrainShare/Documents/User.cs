using System;
using System.Collections.Generic;
using System.Linq;
using BrainShare.Controllers;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Documents
{
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
        //public string FacebookAccessToken { get; set; }

        public AddressData Address { get; set; }

        public List<ChangeRequest> Recieved { get; set; }

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
            Recieved = new List<ChangeRequest>();
            Votes = new Dictionary<string, int>();
            ThreadsWithUnreadMessages = new List<string>();
            Address = new AddressData();
        }

        public void AddRecievedBook(string bookId, string userId)
        {
            Recieved.Add(new ChangeRequest() { BookId = bookId, UserId = userId });
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
    }

    public class AddressData
    {
        public string Original { get; set; }
        public string Formatted { get; set; }
        public string Country { get; set; }
        public string Locality { get; set; }

        public bool IsValid { get; set; }

        public AddressData(RegisterViewModel model)
        {
            Original = model.original_address;
            Formatted = model.formatted_address;
            Country = model.country;
            Locality = model.locality;
            IsValid = true;
        }

        public AddressData()
        {
            
        }

        public AddressData(string fbLocationData)
        {
            Original = fbLocationData;
            Formatted = fbLocationData;
            try
            {
                var arr = fbLocationData.Split(',');
                Country = arr[1].Trim();
                Locality = arr[0].Trim();
                IsValid = true;
            }
            catch
            {
                IsValid = false;
            }
        }
    }

    public class ChangeRequest
    {
        public string UserId { get; set; }
        public string BookId { get; set; }
        public DateTime Created { get; set; }
        public bool Viewed { get; set; }

        public ChangeRequest()
        {
            Created = DateTime.Now;
        }
    }
}