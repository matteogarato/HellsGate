using HellsGate.Lib.Interfaces;
using HellsGate.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HellsGate.Controllers
{
    [Route("PlateVerification")]
    [ApiController]
    public class PlateVerificationController : ControllerBase
    {
        private readonly AuthType AccessType = AuthType.User;//TODO: add configuration reading
        private readonly IAccessManager AccessManager;

        [HttpGet("{PlateNumber}")]
        public async Task<bool> Get(string PlateNumber)
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

            return await AccessManager.Access(newAccess, AccessType);
        }
    }
}