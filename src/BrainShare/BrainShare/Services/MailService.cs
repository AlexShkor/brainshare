using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using BrainShare.Documents;
using BrainShare.ViewModels;

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
            Send(newUser.Email, newUser.FullName, "a.putov@paralect.com", "a.putov@paralect.com", "BrainShare : Благодарим за регистрацию на BrainShare!", @"<p>welcome message</p>");
        }

        public void SendRequestMessage(User currentUser, User requestedUser, Book book)
        {
            var requestViewModel = new RequestViewModel()
                                       {
                                           CurrentUser = currentUser,
                                           RequestedUser = requestedUser,
                                           Book = book
                                       };


            Send(currentUser.Email, currentUser.FullName, "", "", "BrainShare : Уведомление об отправке запроса", @"<p>welcome message</p>");
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

            Send(firstUser.Email, firstUser.FullName, "", "", "BrainShare : Уведомление об обмене", @"<p>welcome message</p>");
        }

        private void Send(string toAddress, string toDisplayName,string fromAddress, string fromDisplayName, string subject, string html,bool async = true)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.BodyEncoding = Encoding.UTF8;
            // To
            mailMsg.To.Add(new MailAddress(toAddress, toDisplayName));

            // From
            mailMsg.From = new MailAddress(fromAddress, fromDisplayName);

            // Subject and multipart/alternative Body
            mailMsg.Subject = subject;
            mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

            // Init SmtpClient and send
            SmtpClient smtpClient = new SmtpClient();
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("f841dc01-7378-4c92-a783-4eecad474217@apphb.com", "p0gxh9sk");
            smtpClient.Credentials = credentials;

            if (async)
            {
                Task.Factory.StartNew(() => smtpClient.Send(mailMsg));        
            }
            else
            {
                smtpClient.Send(mailMsg);
            }
       
        }
    }

}