using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HellsGate.Lib;
using HellsGate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HellsGate.Controllers
{
    [Route("CardVerificationApi")]
    public class CardVerificationApi : Controller
    {
        public AuthType AccessType = AuthType.User;//TODO: add configuration reading
        [HttpGet("{CardId}")]
        public async Task<bool> Get(string CardId)
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
                    if (await context.Peoples.AnyAsync(c => c.CardNumber == CardId))
                    {
                        var entered = await context.Peoples.FirstAsync(a => a.CardNumber == CardId);
                        newAccess.PeopleEntered = entered.Id;
                        newAccess.GrantedAccess = await Lib.AutorizationManager.IsAutorized(newAccess.PeopleEntered, AccessType);
                        accessGranted = true;
                    }
                    else
                    {
                        //TODO: add plate after confirm
                    }
                    await context.Access.AddAsync(newAccess);
                    await context.SaveChangesAsync();
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
