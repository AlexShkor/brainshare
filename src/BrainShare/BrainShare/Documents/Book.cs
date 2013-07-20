using System.Collections.Generic;
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
        public string PublishedDate { get; set; }
        public string Publisher { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }
        public string Country { get; set; }
        public List<string> Authors { get; set; }
        public SortedSet<string> Owners { get; set; }
        public SortedSet<string> Lookers { get; set; }

        public Book()
        {
            ISBN = new List<string>();
            Authors = new List<string>();
            Owners = new SortedSet<string>();
            Lookers = new SortedSet<string>();
        }
    }
}