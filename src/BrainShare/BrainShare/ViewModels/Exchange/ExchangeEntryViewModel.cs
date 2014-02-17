using BrainShare.Domain.Documents.Data;
using BrainShare.Domain.Enums;
using BrainShare.Utils.Utilities;
using Brainshare.Infrastructure.Infrastructure;

namespace BrainShare.ViewModels
{
    public class ExchangeEntryViewModel
    {
        public ExchangeEntryViewModel(ExchangeEntry entry)
        {
            AvatarUrl = entry.User.AvatarUrl ?? Constants.DefaultAvatarUrl;

            if (entry.Action != ExchangeActionEnum.Take && entry.BookSnapshot!= null)
            {
                BookTitle = entry.BookSnapshot.Title;
                BookSubTitle = entry.BookSnapshot.Subtitle;
                BookInfoLink = UrlUtility.GetBookLink(entry.BookSnapshot.Id,UrlUtility.ApplicationBaseUrl);
                BookImage = entry.BookSnapshot.Image;
            }
            UserId = entry.User.UserId;
            UserName = entry.User.UserName;
        }

        public string AvatarUrl { get; set; }
        public string UserId { get; set; }
        public string BookInfoLink { get; set; }
        public string BookTitle { get; set; }
        public string BookSubTitle { get; set; }
        public string UserName { get; set; }
        public string BookImage { get; set; }
    }
}