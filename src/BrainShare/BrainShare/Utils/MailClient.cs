using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using BrainShare.Documents;

namespace BrainShare.Utils
{
    public static class MailClient
    {
        private static readonly SmtpClient Client;

        static MailClient()
        {
            Client = new SmtpClient()
                         {
                             Host = ConfigurationManager.AppSettings["SmtpServer"],
                             Port = Convert.ToInt32(ConfigurationManager.AppSettings["SmtpPort"]),
                             DeliveryMethod = SmtpDeliveryMethod.Network
                         };
            Client.UseDefaultCredentials = false;
            Client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SmtpUser"], ConfigurationManager.AppSettings["SmtpPass"]);
        }

        private static bool SendMessage(string from, string to, string subject, string body)
        {
            MailMessage mail = null;
            bool isSent = false;

            try
            {
                // Create a message
                mail = new MailMessage(from, to, subject, body);
                mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                // Send it
                Client.Send(mail);
                isSent = true;
            }

            catch (Exception ex)
            {
                throw new NotImplementedException();
            }

            finally
            {
                mail.Dispose();
            }

            return isSent;
        }

        public static bool SendWelcome(string email)
        {
            string body = "Wellcome content";

            return SendMessage(ConfigurationManager.AppSettings["adminEmail"], email, "Wellcome header", body);
        }

        public static bool SendRequestMessage(User currentUser, User requestedUser, Book book)
        {

            string header = "BrainShare : Уведомление об отправке запроса";
            string body = "Ваш запрос к пользователю " + requestedUser.FirstName + " " + requestedUser.LastName + " на книгу \"" + book.Title + "\"" + "отрправлен.";

            var isSent =  SendMessage(ConfigurationManager.AppSettings["adminEmail"], currentUser.Email, header, body);

            return isSent;
        }


        public static bool SendExchangeConfirmMessage(User firstUser, Book firstBook, User secondUser, Book secondBook)
        {
            bool isSent = false;
            var header = "BrainShare : Уведомление об обмене";
            var bodyForFirstUser = "Вы успешно обменялись книгой \"" + firstBook.Title + "\" с пользователем " +
                                   secondUser.FirstName + " " + secondUser.LastName + " (" + secondUser.Email +
                                   "). В вашу коллекцию добавлена книга \"" + secondBook.Title + "\"";

            var bodyForSecondUser = "Вы успешно обменялись книгой \"" + secondBook.Title + "\" с пользователем " +
                                    firstUser.FirstName + " " + firstUser.LastName + " (" + firstUser.Email +
                                    "). В вашу коллекцию добавлена книга \"" + firstBook.Title + "\"";

            var sendToFirstUser = SendMessage(ConfigurationManager.AppSettings["adminEmail"], firstUser.Email, header,
                                              bodyForFirstUser);
            var sendToSecondUser = SendMessage(ConfigurationManager.AppSettings["adminEmail"], secondUser.Email, header,
                                               bodyForSecondUser);
            
            if (sendToFirstUser == true && sendToSecondUser == true)
            {
                isSent = true;
            }

            return isSent;
        }
    }
}