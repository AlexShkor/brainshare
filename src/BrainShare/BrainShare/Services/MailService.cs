using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using ActionMailer.Net.Mvc;
using BrainShare.Documents;
using BrainShare.ViewModels;

namespace BrainShare.Services
{
    public class MailService : MailerBase
    {

        public EmailResult SendWelcomeMessage(User newUser)
        {
            To.Add(newUser.Email);
            Subject = "BrainShare : Благодарим за регистрацию на BrainShare!";
            MessageEncoding = Encoding.UTF8;
            return Email("WelcomeMessage", newUser);
        }

        public EmailResult SendWelcomeMessage(ShellUser newUser)
        {
            To.Add(newUser.Email);
            Subject = "BrainShare : Благодарим за регистрацию на BrainShare!";
            MessageEncoding = Encoding.UTF8;
            return Email("WelcomeMessage", newUser);
        }

        public EmailResult SendRequestMessage(User currentUser, User requestedUser, Book book)
        {
            var requestViewModel = new RequestViewModel()
                                       {
                                           CurrentUser = currentUser,
                                           RequestedUser = requestedUser,
                                           Book = book
                                       };
            To.Add(currentUser.Email);
            Subject = "BrainShare : Уведомление об отправке запроса";
            MessageEncoding = Encoding.UTF8;
            return Email("RequestMessage", requestViewModel);
        }

        public EmailResult SendExchangeConfirmMessage(User firstUser, Book firstBook, User secondUser, Book secondBook)
        {
            var exchangeViewModel = new ExchangeConfirmViewModel()
                                        {
                                            You = firstUser,
                                            YourBook = firstBook,
                                            He = secondUser,
                                            HisBook = secondBook
                                        };

            To.Add(firstUser.Email);
            Subject = "BrainShare : Уведомление об обмене";
            MessageEncoding = Encoding.UTF8;
            return Email("ExchangeConfirmMessage", exchangeViewModel);
        }


    }

}