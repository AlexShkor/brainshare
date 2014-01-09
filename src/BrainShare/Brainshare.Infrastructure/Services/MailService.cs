using System.Globalization;
using BrainShare.Domain.Documents;
using BrainShare.EmailMessaging;
using BrainShare.EmailMessaging.ViewModels;
using BrainShare.Infostructure;
using BrainShare.Utils.Utilities;
using Brainshare.Infrastructure.Infrastructure;

namespace Brainshare.Infrastructure.Services
{
    public class MailService
    {
        private readonly Settings.Settings _settings;
        private readonly Emailer _emailer;

        public MailService(Settings.Settings settings)
        {
            _settings = settings;
            _emailer = new Emailer(_settings.AdminEmail,_settings.AdminDisplayName);
        }

        public void SendWelcomeMessage(string fullName, string email,string confirmLink = null)
        {
           _emailer.SendWelcomeMessage(new Welcome
               {
                   ReceiverName = fullName,
                   ConfirmLink = confirmLink
               },
               email, fullName);
        }

        public void SendGiftExchangeMessage(Book book, User owner, User receiver)
        {
            _emailer.SendGiftExchangeMessage(new GiftExchange
            {
                OwnerFullName = owner.FullName,
                OwnerProfileLink = UrlUtility.GetProfileLink(owner.Id),
                BookLink = UrlUtility.GetProfileLink(book.Id),
                BookTitle = book.Title
            },
             receiver.Email, receiver.FullName);
        }

        public void EmailUserMessage(string message, User sender, User receiver)
        {
            _emailer.EmailUserMessage(new UserMessage
                {
                    Message = message ,
                    SenderFullName = sender.FullName,
                    SenderProfileLink = UrlUtility.GetProfileLink(sender.Id),                 
                },
                receiver.Email,receiver.FullName );
        }

        public void EmailUserHaveSearechedBook(User owner, User receiver,  Book book)
        {
             _emailer.EmailUserHaveSearechedBook(new UserHaveSearechedBook
                 {
                    Authors = string.Join(", ", book.Authors),
                    BookImage = book.Image ?? Constants.DefaultBookImage,
                    BookTitle = book.Title,
                    OwnerFullName = owner.FullName,
                    OwnerLocality = owner.Address.Locality,
                    OwnerProfileLink = UrlUtility.GetProfileLink(owner.Id),
                    PageCount = book.PageCount.ToString(),
                    PublishedDate = book.PublishedYear != null ? book.PublishedDate.ToString("yyyy MMM", CultureInfo.GetCultureInfo("ru")) : null
                 }, 
                 receiver.Email, receiver.FullName);
        }

        public void SendRequestMessage(User currentUser, User requestedUser, Book book)
        {
            _emailer.SendRequestMessage(new Request
                {
                   BookTitle = book.Title,
                   BookLink = UrlUtility.GetBookLink(book.Id),
                   RequestedUserFullName = requestedUser.FullName,
                   RequestedUserProfileLink = UrlUtility.GetProfileLink(requestedUser.Id)
                },
                requestedUser.Email,requestedUser.FullName);
        }

        public void SendExchangeConfirmMessage(User firstUser, Book firstBook, User secondUser, Book secondBook)
        {
           _emailer.SendExchangeConfirmMessage(new ExchangeConfirm
               {
                   MyBookTitle = firstBook.Title,
                   PartnerBookTitle = secondBook.Title,
                   PartnerEmail = secondUser.Email,
                   PartnerFullName = secondUser.FullName
               }, 
               firstUser.Email,firstUser.FullName);
        }
    }

}