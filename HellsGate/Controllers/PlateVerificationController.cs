using HellsGate.Lib;
using HellsGate.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace HellsGate.Controllers
{
    [Route("PlateVerification")]
    [ApiController]
    public class PlateVerificationController : ControllerBase
    {
        private readonly AuthType AccessType = AuthType.User;//TODO: add configuration reading

        // GET: api/IsAuthorized/5
        [HttpGet("{PlateNumber}")]
        public async Task<bool> IsAuthorized(string PlateNumber)
        {
            if (string.IsNullOrEmpty(PlateNumber) || string.IsNullOrEmpty(PlateNumber.Trim()))
            {
                return false;
            }
            var newAccess = new AccessModel
            {
                AccessTime = DateTime.Now,
                GrantedAccess = false,
                Plate = PlateNumber
            };
            using (var context = new HellsGateContext())
            {
                try
                {
                    if (await context.Cars.AnyAsync(a => a.LicencePlate == PlateNumber).ConfigureAwait(false))
                    {
                        newAccess.GrantedAccess = await AutorizationManager.IsPeopleAutorized(PlateNumber, AccessType).ConfigureAwait(false);
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