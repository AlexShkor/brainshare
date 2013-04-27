using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

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

        public static bool SendExchangeConfirmMessage()
        {
            return false;
        }
    }
}