using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using BrainShare.Documents;
using BrainShare.ViewModels;
using RazorEngine;
using Encoding = System.Text.Encoding;

namespace BrainShare.Services
{
    public class MailService
    {
        private readonly Settings _settings;

        public MailService(Settings settings)
        {
            _settings = settings;
        }

        public void SendWelcomeMessage(User newUser)
        {
           var body = GetStringFromRazor("WelcomeMessage", newUser);
           Send(newUser.Email, newUser.FullName, "BrainShare : Благодарим за регистрацию на BrainShare!", body);
        }

        public void EmailUserMessage(string message, User sender, User receiver)
        {
            var model = new EmailUserMessageModel {Message = message, Sender = sender};
            var body = GetStringFromRazor("EmailUserMessage", model);
            Send(receiver.Email, receiver.FullName, "BrainShare : Новое сообщение", body);
        }

        public void SendRequestMessage(User currentUser, User requestedUser, Book book)
        {
            var requestViewModel = new RequestViewModel()
                                       {
                                           CurrentUser = currentUser,
                                           RequestedUser = requestedUser,
                                           Book = book
                                       };

            var body = GetStringFromRazor("RequestMessage", requestViewModel);
            Send(currentUser.Email, currentUser.FullName, "BrainShare : Уведомление об отправке запроса", body);
        }

        public void SendExchangeConfirmMessage(User firstUser, Book firstBook, User secondUser, Book secondBook)
        {
            var exchangeViewModel = new ExchangeConfirmViewModel()
                                        {
                                            You = firstUser,
                                            YourBook = firstBook,
                                            He = secondUser,
                                            HisBook = secondBook
                                        };

            var body = GetStringFromRazor("ExchangeConfirmMessage", exchangeViewModel);
            Send(firstUser.Email, firstUser.FullName, "BrainShare : Уведомление об обмене", body);
        }

        private void Send(string toAddress, string toDisplayName, string subject, string html,bool async = true)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.BodyEncoding = Encoding.UTF8;
            // To
            mailMsg.To.Add(new MailAddress(toAddress, toDisplayName));

            // From
            mailMsg.From = new MailAddress(_settings.AdminEmail, _settings.AdminDisplayName);

            // Subject and multipart/alternative Body
            mailMsg.Subject = subject;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            // Init SmtpClient and send
            SmtpClient smtpClient = new SmtpClient();

            if (async)
            {
                Task.Factory.StartNew(() => smtpClient.Send(mailMsg));        
            }
            else
            {
                smtpClient.Send(mailMsg);
            }      
        }

        private string GetStringFromRazor(string viewname, object model)
        {
            var path = System.Web.HttpContext.Current.Server.MapPath(@"~/Views/MailService/" + viewname + ".cshtml");
            var fileContents = System.IO.File.ReadAllText(path);

            return Razor.Parse(fileContents, model);
        }
    }

}