using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HellsGate.Lib;
using HellsGate.Models;
using Microsoft.AspNetCore.Mvc;


namespace HellsGate.Controllers
{
    [Route("CardVerificationApi")]
    public class CardVerificationApi : Controller
    {
        public AuthType AccessType = AuthType.User;//TODO: add configuration reading
        [HttpGet("{CardId}")]
        public bool Get(string CardId)
        {
            if (string.IsNullOrEmpty(CardId) || string.IsNullOrEmpty(CardId.Trim()))
            {
                return false;
            }
            bool accessGranted = false;
            using (var context = new Context())
            {
                AccessModel newAccess = new AccessModel
                {
                    AccessTime = DateTime.Now,
                };
                try
                {
                    if (context.Cards.Any(c => c.CardNumber == CardId))
                    {
                        newAccess.PeopleEntered = context.Cards.First(a => a.CardNumber == CardId).people;
                        newAccess.GrantedAccess = Lib.AutorizationManager.IsAutorized(newAccess.CarEntered, AccessType);
                        accessGranted = true;
                    }
                    else
                    {
                        //TODO: add plate after confirm
                    }
                    context.Access.Add(newAccess);
                    context.SaveChanges();
                    StaticEventHandler.SendMail(new MailEventArgs(ResourceString.AccessCarMailSubject, ResourceString.AccessCarMailBody, DateTime.Now));
                }
                catch (Exception ex)
                {
                    StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during plate verification", MethodBase.GetCurrentMethod(), ex);
                }
            }
            return accessGranted;
        }

    }
}
