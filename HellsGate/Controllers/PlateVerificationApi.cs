using System;
using System.Reflection;
using System.Threading.Tasks;
using HellsGate.Lib;
using HellsGate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HellsGate.Controllers
{
    [Route("PlateVerificationApi")]
    public class PlateVerificationApi : Controller
    {
        private readonly AuthType AccessType = AuthType.User;//TODO: add configuration reading
        // GET api/<controller>/5
        [HttpGet("{PlateNumber}")]
        public async Task<bool> Get(string platenumber)
        {
            if (string.IsNullOrEmpty(platenumber) || string.IsNullOrEmpty(platenumber.Trim()))
            {
                return false;
            }
            var newAccess = new AccessModel
            {
                AccessTime = DateTime.Now,
                GrantedAccess = false,
                Plate = platenumber
            };
            using (var context = new Context())
            {
                try
                {
                    if (await context.Cars.AnyAsync(a => a.LicencePlate == platenumber).ConfigureAwait(false))
                    {
                        newAccess.GrantedAccess = await AutorizationManager.IsAutorized(platenumber, AccessType).ConfigureAwait(false);
                    }
                    await context.Access.AddAsync(newAccess).ConfigureAwait(false);
                    await context.SaveChangesAsync().ConfigureAwait(false);
                    //StaticEventHandler.SendMail(new MailEventArgs(ResourceString.AccessCarMailSubject, ResourceString.AccessCarMailBody,, DateTime.Now));
                }
                catch (Exception ex)
                {
                    StaticEventHandler.Log(System.Diagnostics.TraceLevel.Error, "error during plate verification", MethodBase.GetCurrentMethod(), ex);
                }
            }
            return newAccess.GrantedAccess;
        }

    }
}
