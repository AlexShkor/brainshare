using BrainShare.Domain.Documents;
using Brainshare.Infrastructure.Infrastructure;
using BrainShare.Utils.Utilities;

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
        public string UserLocality { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsUserReadMe { get; set; }

        public string CurrentUserId { get; set; }

        public string SharingSearchBookText
        {
            get { return UserName + " ищет книгу \"" + Title + "\" на BrainShare"; }
        }

        public string SharingHaveBookText
        {
            get { return "У пользователя " + UserName + " есть книга \"" + Title + "\" на BrainShare"; }
        }

        public string UserComment { get; set; }

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
            Image = book.Image ?? Constants.DefaultBookImage;
            GoogleBookId = book.GoogleBookId;
            Authors = string.Join(", ", book.Authors);
            Address = book.UserData.Address.Locality;
            UserId = book.UserData.UserId;
            UserName = book.UserData.UserName;
            UserProfile = "/profile/view/" + book.UserData.UserId;
            UserLocality = book.UserData.Address.Locality;
            AvatarUrl =  UrlUtility.ResizeAvatar(book.UserData.AvatarUrl, 64) ?? Constants.DefaultAvatarUrl;
            IsUserReadMe = book.IsUserReadMe;
            UserComment = book.UserComment ?? "(нет комментария владельца)";
        }

        public BookViewModel()
        {

        }
    }
}