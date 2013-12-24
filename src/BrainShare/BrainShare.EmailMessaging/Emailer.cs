using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Hosting;
using BrainShare.EmailMessaging.ViewModels;
using RazorEngine;
using Encoding = System.Text.Encoding;

namespace BrainShare.EmailMessaging
{
    public class Emailer
    {
        private string _adminEmail;
        private string _adminDisplayName;

        public Emailer(string adminEmail, string adminDisplayName)
        {
            _adminEmail = adminEmail;
            _adminDisplayName = adminDisplayName;
        }

        public void SendWelcomeMessage(Welcome welcomModel,string email,string displayName)
        {
           var body = GetStringFromRazor("WelcomeMessage", welcomModel);
           Send(email, displayName, "BrainShare : Благодарим за регистрацию на BrainShare!", body);
        }

        public void SendGiftExchangeMessage(GiftExchange giftExchangeModel,string email,string displayName)
        {
            var body = GetStringFromRazor("GiftExchangeMessage", giftExchangeModel);
            Send(email, displayName , "BrainShare : Вам подарили книгу!!", body);
        }

        public void EmailUserMessage(UserMessage userMessageModel, string email, string displayName)
        {
            var body = GetStringFromRazor("EmailUserMessage", userMessageModel);
            Send(email, displayName, "BrainShare : Новое сообщение", body);
        }

        public void EmailUserHaveSearechedBook(UserHaveSearechedBook userHaveSearechedBookModel, string email, string displayName)
        {
            var body = GetStringFromRazor("EmailUserHaveSearechedBookMessage", userHaveSearechedBookModel);
            Send(email, displayName, "BrainShare : Интересующая", body);
        }

        public void SendRequestMessage(Request requestModel, string email, string displayName)
        {
            var body = GetStringFromRazor("RequestMessage", requestModel);
            Send(email, displayName, "BrainShare : Уведомление об отправке запроса", body);
        }

        public void SendExchangeConfirmMessage(ExchangeConfirm exchangeConfirmModel, string email, string displayName)
        {
            var body = GetStringFromRazor("ExchangeConfirmMessage", exchangeConfirmModel);
            Send(email, displayName, "BrainShare : Уведомление об обмене", body);
        }

        private void Send(string toAddress, string toDisplayName, string subject, string html,bool async = true)
        {
            MailMessage mailMsg = new MailMessage();
            mailMsg.BodyEncoding = Encoding.UTF8;
            // To
            mailMsg.To.Add(new MailAddress(toAddress, toDisplayName));

            // From
            mailMsg.From = new MailAddress(_adminEmail, _adminDisplayName);

            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html);

            // Subject and multipart/alternative Body
            mailMsg.Subject = subject;
            mailMsg.AlternateViews.Add(htmlView);

            // Init SmtpClient and send
            using (var smtpClient = new SmtpClient())
            {
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

        private string GetStringFromRazor(string viewname, object model)
        {
            var path = System.Web.HttpContext.Current.Server.MapPath(string.Format("Views/{0}.cshtml", viewname));
            var fileContents = System.IO.File.ReadAllText(path);

            return Razor.Parse(fileContents, model);
        }
    }

}