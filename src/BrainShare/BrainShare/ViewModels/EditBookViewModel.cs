using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using BrainShare.Documents;
using BrainShare.ViewModels.Base;
using Brainshare.Infrastructure.Extensions;
using Brainshare.Infrastructure.Infrastructure;
using Brainshare.Infrastructure.Services.Validation;

namespace BrainShare.ViewModels
{
    public class EditBookViewModel : BaseViewModel
    {
        public const string DateFormat = "yyyy MMM";
        public static CultureInfo Culture { get { return CultureInfo.GetCultureInfo("ru"); } }

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
        public string Language { get; set; }
        public bool IsWhishBook { get; set; }
        public IEnumerable<LanguageInfo> Languages { get; set; }

        public EditBookViewModel(Book book, IEnumerable<LanguageInfo> languages):this()
        {
            Id = book.Id;
            if (book.ISBN.Any())
            {
                ISBNs = book.ISBN.Select(x => new StringItem(x)).ToList();
            }
            Title = book.Title;
            SearchInfo = book.SearchInfo;
            PageCount = book.PageCount;
            PublishedDate = book.PublishedDate.ToString(DateFormat, Culture);
            Publisher = book.Publisher;
            Subtitle = book.Subtitle;
            Image = book.Image;
            if (book.Authors.Any())
            {
                Authors = book.Authors.Select(x => new StringItem(x)).ToList();
            }
            Language = book.Language;
            Languages = languages;
        }

        public EditBookViewModel()
        {
            ISBNs = new List<StringItem> { new StringItem() };
            Authors = new List<StringItem> { new StringItem() };
            Image = Constants.DefaultBookImage;
        }

        public EditBookViewModel(IEnumerable<LanguageInfo> languages)
            : this()
        {
            Languages = languages;

        }

        public void UpdateBook(Book book)
        {
            book.Title = Title;
            if (ISBNs != null && ISBNs.Any(x => x.Value.HasValue()))
            {
                book.ISBN = ISBNs .Select(x => x.Value).ToList();
            }
            book.SearchInfo = SearchInfo;
            book.Title = Title;
            book.Language = Language;
            if (Authors != null && Authors.Any(x => x.Value.HasValue()))
            {
                book.Authors = Authors.Select(x => x.Value).ToList();
            }
            book.Image = Image;
            book.Subtitle = Subtitle;
            book.Publisher = Publisher;
            book.PageCount = PageCount;

            try
            {
                DateTime dt = DateTime.ParseExact(PublishedDate,
                    DateFormat,
                    Culture,
                    DateTimeStyles.None);

                book.PublishedYear = dt.Date.Year;
                book.PublishedMonth = dt.Date.Month;
            }
            catch
            {
                book.PublishedYear = DateTime.Now.Year;
            }
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