using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HellsGate.Lib;
using HellsGate.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HellsGate.Controllers
{
    [Route("api/PlateVerificationApi")]
    public class PlateVerificationApi : Controller
    {
        public AuthType AccessType = AuthType.User;//TODO: add configuration reading
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        [HttpGet("{platenumber}")]
        public bool Get(string platenumber)
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
                    Plate = platenumber
                };
                try
                {
                    if (context.Cars.Any(a => a.LicencePlate == platenumber))
                    {
                        newAccess.CarEntered = context.Cars.First(a => a.LicencePlate == platenumber);
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

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
