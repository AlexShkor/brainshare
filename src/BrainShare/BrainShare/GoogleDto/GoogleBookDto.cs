using System.Collections.Generic;
using BrainShare.Documents;
using MongoDB.Bson;

namespace BrainShare.GoogleDto
{
    public class GoogleBookDto
    {
            public string Id { get; set; }

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
            public List<string> ISBNS { get; set; }


        public Book BuildDocument()
        {
            return new Book
            {
                Id = ObjectId.GenerateNewId().ToString(),
                GoogleBookId = Id,
                Authors = Authors,
                ISBN = ISBNS ?? new List<string>(),
                Country = Country,
                Image = Image,
                Language = Language,
                Title = Title,
                PageCount = PageCount,
                PublishedDate = PublishedDate,
                Subtitle = Subtitle,
                SearchInfo = SearchInfo,
                Publisher = Publisher
            };
        }
    }
}