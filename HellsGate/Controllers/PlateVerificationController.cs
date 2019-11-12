using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HellsGate.Lib;
using HellsGate.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HellsGate.Controllers
{
    [Route("api/PlateVerification")]
    [ApiController]
    public class PlateVerificationController : ControllerBase
    {
    private readonly AuthType AccessType = AuthType.User;//TODO: add configuration reading
        // GET: api/PateVerification
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/IsAuthorized/5
        [HttpGet("{PlateNumber}")]
        [Route("IsAuthorized")]
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
            using (var context = new Context())
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

        // POST: api/PateVerification
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/PateVerification/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
