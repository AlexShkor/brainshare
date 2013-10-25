using System.Web.Mvc;
using BrainShare.Controllers;
using BrainShare.Documents;

namespace BrainShare.ViewModels
{
    public class BookViewModel
    {
        public string Id { get; set; }
        public string GoogleBookId { get; set; }
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Authors { get; set; }
        public string SearchInfo { get; set; }
        public int? PageCount { get; set; }
        public string PublishedDate { get; set; }
        public string Publisher { get; set; }
        public string Subtitle { get; set; }
        public string Image { get; set; }
        public string Address { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfile { get; set; }
        public string AvatarUrl { get; set; }

        public string CurrentUserId { get; set; }

        public string SharingText
        {
            get { return UserName + " ищет книгу \"" + Title + "\" на BrainShare"; }
        }

        public BookViewModel(Book book)
        {
            Id = book.Id;
            ISBN = string.Join(", ", book.ISBN);
            Title = book.Title;
            SearchInfo = book.SearchInfo;
            PageCount = book.PageCount;
            PublishedDate = book.PublishedYear != null ? book.PublishedDate.ToString(EditBookViewModel.DateFormat, EditBookViewModel.Culture) : null;
            Publisher = book.Publisher;
            Subtitle = book.Subtitle;
            Image = book.Image;
            GoogleBookId = book.GoogleBookId;
            Authors = string.Join(", ", book.Authors);
            Address = book.UserData.Address.Formatted;
            UserId = book.UserData.UserId;
            UserName = book.UserData.UserName;
            UserProfile = "/profile/view/" + book.UserData.UserId;
            AvatarUrl = book.UserData.AvatarUrl;
        }

        public BookViewModel()
        {

        }
    }
}