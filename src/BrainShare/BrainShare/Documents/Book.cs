using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace BrainShare.Documents
{
    public class Book
    {
        public string Id { get; set; }

        public string ISBN { get; set; }
        public string Title { get; set; }
        public string SearchInfo { get; set; }
        public string Language { get; set; }
        public int PageCount { get; set; }
        public string PublishedDate { get; set; }
        public string Publisher { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }
        public List<string> Authors { get; set; }
        public List<string> Owners { get; set; }

        public Book()
        {
            Authors = new List<string>();
            Owners = new List<string>();
        }
    }
}