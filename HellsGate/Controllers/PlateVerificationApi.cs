using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HellsGate.Lib;
using HellsGate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HellsGate.Controllers
{
    [Route("PlateVerificationApi")]
    public class PlateVerificationApi : Controller
    {
        public AuthType AccessType = AuthType.User;//TODO: add configuration reading
        // GET api/<controller>/5
        [HttpGet("{PlateNumber}")]
        public async Task<bool> Get(string platenumber)
        {
            if (string.IsNullOrEmpty(platenumber) || string.IsNullOrEmpty(platenumber.Trim()))
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
                    if (await context.Cars.AnyAsync(a => a.LicencePlate == platenumber))
                    {
                        newAccess.Plate = platenumber;
                        newAccess.GrantedAccess = await Lib.AutorizationManager.IsAutorized(platenumber, AccessType);
                        accessGranted = true;
                    }
                    else
                    {
                        //TODO: add plate after confirm
                    }
                    await context.Access.AddAsync(newAccess);
                    await context.SaveChangesAsync();
                    //StaticEventHandler.SendMail(new MailEventArgs(ResourceString.AccessCarMailSubject, ResourceString.AccessCarMailBody,, DateTime.Now));
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
