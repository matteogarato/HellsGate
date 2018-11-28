﻿using HellsGate.Models;
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

        readonly Startup startup;

        public MailSenderManager(Startup p_startup)
        {
            startup = p_startup;
            client = new SmtpClient
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("username", "password")
            };
            startup.SendMailEvent += SendMessage;
        }

        private void SendMessage(object sender, MailEventArgs p_mailEvent)
        {
            try
            {
                mailMessage = new MailMessage
                {
                    From = new MailAddress("whoever@me.com")
                };
                mailMessage.To.Add("receiver@me.com");
                mailMessage.Body = p_mailEvent.Message;
                mailMessage.Subject = p_mailEvent.Subject;
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {

            }
        }
    }
}