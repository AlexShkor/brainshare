using System;
using System.Collections.Generic;
using System.Globalization;
using BrainShare.Documents;
using MongoDB.Bson;

namespace BrainShare.GoogleDto
{
    public class GoogleBookDto
    {
        public string GoogleBookId { get; set; }

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


        public Book BuildDocument(User user)
        {
            var book = new Book
            {
                Id = ObjectId.GenerateNewId().ToString(),
                GoogleBookId = GoogleBookId,
                Authors = Authors ?? new List<string>(),
                ISBN = ISBNS ?? new List<string>(),
                Country = Country,
                Image = Image,
                Language = Language,
                Title = Title,
                PageCount = PageCount,
                Subtitle = Subtitle,
                SearchInfo = SearchInfo,
                Publisher = Publisher
            };
            try
            {
                var dateParts = PublishedDate.Split('-');
                book.PublishedYear = int.Parse(dateParts[0]);
                book.PublishedMonth = int.Parse(dateParts[1]);
                book.PublishedDay = int.Parse(dateParts[2]);
            }
            catch
            {
            }
            book.UserData = new UserData(user);
            return book;
        }

    }
}