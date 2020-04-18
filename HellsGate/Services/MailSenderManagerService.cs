using HellsGate.Models;
using System;
using System.Net;
using System.Net.Mail;
using System.Reflection;

namespace HellsGate.Services
{
    public class MailSenderManagerService
    {
        private readonly SmtpClient client = new SmtpClient();
        private MailMessage mailMessage = new MailMessage();

        public MailSenderManagerService()
        {
            client = new SmtpClient
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("username", "password")
            };
            StaticEventHandler.SendMailEvent += SendMessage;
        }

        private void SendMessage(object sender, MailEventArgs p_mailEvent)
        {
            try
            {
                mailMessage = new MailMessage
                {
                    From = new MailAddress("whoever@me.com")
                };
                foreach (var rec in p_mailEvent.Recievers)
                {
                    mailMessage.To.Add(rec);
                }
                mailMessage.Body = p_mailEvent.Message;
                mailMessage.Subject = p_mailEvent.Subject;
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "Error Sending mail", MethodBase.GetCurrentMethod(), ex);
            }
        }
    }
}