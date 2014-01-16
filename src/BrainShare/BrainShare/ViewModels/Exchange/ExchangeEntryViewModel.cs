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

            if (entry.ExchangeEntryType != ExchangeEntryType.GiftReceiver && entry.BookSnapshot!= null)
            {
                BookTitle = entry.BookSnapshot.Title;
                BookSubTitle = entry.BookSnapshot.Subtitle;
                BookInfoLink = UrlUtility.GetBookLink(entry.BookSnapshot.Id,UrlUtility.ApplicationBaseUrl);
            }

            ExchangeEntryType = entry.ExchangeEntryType;
        }

        public string AvatarUrl { get; set; }
        public string BookInfoLink { get; set; }
        public string BookTitle { get; set; }
        public string BookSubTitle { get; set; }
        public ExchangeEntryType ExchangeEntryType { get; set; }
    }
}