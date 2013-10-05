using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BrainShare.Documents;
using BrainShare.Services.Validation;
using BrainShare.ViewModels.Base;

namespace BrainShare.Controllers
{
    public class EditBookViewModel : BaseViewModel
    {
        public string Id { get; set; }
        public string GoogleBookId { get; set; }
        public List<StringItem> ISBNs { get; set; }
        [Required]
        public string Title { get; set; }
        public List<StringItem> Authors { get; set; }
        public string SearchInfo { get; set; }
        public int PageCount { get; set; }
        public string PublishedDate { get; set; }
        public string Publisher { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }
        [Required]
        public string Language { get; set; }
        public bool IsWhishBook { get; set; }
        public IEnumerable<LanguageInfo> Languages { get; set; }

        public EditBookViewModel(Book book, IEnumerable<LanguageInfo> languages)
        {
            Id = book.Id;
            ISBNs = book.ISBN.Select(x=> new StringItem(x)).ToList();
            Title = book.Title;
            SearchInfo = book.SearchInfo;
            PageCount = book.PageCount;
            PublishedDate = book.PublishedDate.ToString("yyyy MMM");
            Publisher = book.Publisher;
            Subtitle = book.Subtitle;
            Image = book.Image;
            GoogleBookId = book.GoogleBookId;
            Authors = book.Authors.Select(x => new StringItem(x)).ToList();
            Language = book.Language;
            Languages = languages;
        }

        public EditBookViewModel()
        {
            ISBNs = new List<StringItem> { new StringItem() };
            Authors = new List<StringItem> { new StringItem() };
            Image = "/images/book-logo-placeholder.png";
        }

        public EditBookViewModel(IEnumerable<LanguageInfo> languages):this()
        {
            Languages = languages;
        }

        public void UpdateBook(Book book)
        {
            book.Title = Title;
            book.ISBN = ISBNs.Select(x=> x.Value).ToList();
            book.SearchInfo = SearchInfo;
            book.Title = Title;
            book.GoogleBookId = GoogleBookId;
            book.Language = Language;
            book.Authors = Authors.Select(x => x.Value).ToList();
            book.Image = Image;
            book.Subtitle = Subtitle;
            book.Publisher = Publisher;
            book.PageCount = PageCount;
        }
    }

    public class StringItem
    {
        public string Value { get; set; }

        public StringItem()
        {
            
        }

        public StringItem(string value)
        {
            Value = value;
        }
    }
}