using BrainShare.Documents;

namespace BrainShare.Controllers
{
    public class BookViewModel
    {
        public string Id { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string SearchInfo { get; set; }
        public int PageCount { get; set; }
        public string PublishedDate { get; set; }
        public string Publisher { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }

        public BookViewModel(Book book)
        {
            Id = book.Id;
            ISBN = book.ISBN;
            Title = book.Title;
            SearchInfo = book.SearchInfo;
            PageCount = book.PageCount;
            PublishedDate = book.PublishedDate;
            Publisher = book.Publisher;
            Subtitle = book.Subtitle;
            Image = book.Image;
            Authors = string.Join(", ", book.Authors);
        }
    }
}