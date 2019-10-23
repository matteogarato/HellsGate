using System;
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
        private readonly AuthType AccessType = AuthType.User;//TODO: add configuration reading
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
                var newAccess = new AccessModel
                {
                    AccessTime = DateTime.Now,
                };
                try
                {
                    if (await context.Peoples.AnyAsync(c => c.CardNumber == CardId).ConfigureAwait(false))
                    {
                        PeopleAnagraphicModel entered = await context.Peoples.FirstAsync(a => a.CardNumber == CardId).ConfigureAwait(false);
                        newAccess.PeopleEntered = entered.Id;
                        newAccess.GrantedAccess = await Lib.AutorizationManager.IsAutorized(newAccess.PeopleEntered, AccessType).ConfigureAwait(false);
                        accessGranted = true;
                    }
                    else
                    {
                        //TODO: add plate after confirm
                    }
                    await context.Access.AddAsync(newAccess).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                    //StaticEventHandler.SendMail(new MailEventArgs(ResourceString.AccessCarMailSubject, ResourceString.AccessCarMailBody, DateTime.Now));
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
