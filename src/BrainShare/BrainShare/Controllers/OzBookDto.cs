using System;
using System.Collections.Generic;
using System.Linq;
using BrainShare.Documents;
using BrainShare.Domain.Documents;
using MongoDB.Bson;

namespace BrainShare.Controllers
{
    public class OzBookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Year { get; set; }
        public string Image { get; set; }
        public List<string> Authors { get; set; }
        public string ISBN { get; set; }
        public string Id { get; set; }

        public Book BuildDocument(User user)
        {
            var book = new Book
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Authors = (Authors ?? new List<string>()).Select(x=> x.Trim()).ToList(),
                Image = Image,
                Title = Title,
                SearchInfo = Description,
                OzBookId = Id
            };
            try
            {
                book.PublishedYear = int.Parse(Year);
            }
            catch
            {
                book.PublishedYear = DateTime.Now.Year;
            }
            book.UserData = new UserData(user);
            book.FromOzBy = true;
            return book;
        }
    }
}