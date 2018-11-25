using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace HellsGate.Lib
{
    public class MailSenderManager
    {
        private SmtpClient client = new SmtpClient();
        private MailMessage mailMessage = new MailMessage();

        public MailSenderManager()
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("username", "password");
        }
        public bool SendMessage()
        {
            try
            {
                mailMessage = new MailMessage();
                mailMessage.From = new MailAddress("whoever@me.com");
                mailMessage.To.Add("receiver@me.com");
                mailMessage.Body = "body";
                mailMessage.Subject = "subject";
                client.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
