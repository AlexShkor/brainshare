using BrainShare.Domain.Documents.Data;
using Brainshare.Infrastructure.Infrastructure;

namespace BrainShare.ViewModels
{
    public class ExchangeEntryViewModel
    {
        public ExchangeEntryViewModel(ExchangeEntry entry)
        {
            AvatarUrl = entry.User.AvatarUrl ?? Constants.DefaultAvatarUrl;
            BookTitle = entry.BookSnapshot.Title;
            BookSubTitle = entry.BookSnapshot.Subtitle;
            BookInfoLink = Utils.Utilities.UrlUtility.GetBookLink(entry.BookSnapshot.Id);
        }

        public string AvatarUrl { get; set; }
        public string BookInfoLink { get; set; }
        public string BookTitle { get; set; }
        public string BookSubTitle { get; set; }  
    }
}