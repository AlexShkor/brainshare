using System;
using System.Collections.Generic;
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

        public Dictionary<string, int> Votes { get; set; }

        public string FacebookId { get; set; }

        public string City { get; set; }

        public SortedSet<string> Books { get; set; }
        public SortedSet<string> WishList { get; set; }
        public List<ChangeRequest> Recieved { get; set; }

       
        public List<ChangeRequest> Inbox { get; set; }

        public string FullName
        {
            get { return string.Format("{0} {1}", FirstName, LastName); }
        }

        public User()
        {
            Books = new SortedSet<string>();
            WishList = new SortedSet<string>();
            Inbox = new List<ChangeRequest>();
            Recieved = new List<ChangeRequest>();
            Votes = new Dictionary<string, int>();
            ThreadsWithUnreadMessages = new List<string>();
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