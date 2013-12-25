using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
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
           var body = GetStringFromRazor("Welcome", welcomModel);
           Send(email, displayName, "BrainShare : Благодарим за регистрацию на BrainShare!", body);
        }

        public void SendGiftExchangeMessage(GiftExchange giftExchangeModel,string email,string displayName)
        {
            var body = GetStringFromRazor("GiftExchange", giftExchangeModel);
            Send(email, displayName , "BrainShare : Вам подарили книгу!!", body);
        }

        public void EmailUserMessage(UserMessage userMessageModel, string email, string displayName)
        {
            var body = GetStringFromRazor("UserMessage", userMessageModel);
            Send(email, displayName, "BrainShare : Новое сообщение", body);
        }

        public void EmailUserHaveSearechedBook(UserHaveSearechedBook userHaveSearechedBookModel, string email, string displayName)
        {
            var body = GetStringFromRazor("UserHaveSearechedBook", userHaveSearechedBookModel);
            Send(email, displayName, "BrainShare : Интересующая", body);
        }

        public void SendRequestMessage(Request requestModel, string email, string displayName)
        {
            var body = GetStringFromRazor("Request", requestModel);
            Send(email, displayName, "BrainShare : Уведомление об отправке запроса", body);
        }

        public void SendExchangeConfirmMessage(ExchangeConfirm exchangeConfirmModel, string email, string displayName)
        {
            var body = GetStringFromRazor("ExchangeConfirm", exchangeConfirmModel);
            Send(email, displayName, "BrainShare : Уведомление об обмене", body);
        }

        private async void Send(string toAddress, string toDisplayName, string subject, string html,bool async = true)
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

            if (async)
            {
                ThreadPool.QueueUserWorkItem(t => Send(mailMsg));
            }
            else
            {
                Send(mailMsg);
            }     
        }

        private void Send(MailMessage msg)
        {
            using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Send(msg);
                }
        }

        public string GetStringFromRazor(string viewname, object model)
        {
            var basePath = AppDomain.CurrentDomain.RelativeSearchPath;
            var path = Path.Combine(basePath, "Views", viewname + ".cshtml");

            var fileContents = System.IO.File.ReadAllText(path);
            return Razor.Parse(fileContents, model);
        }
    }

}