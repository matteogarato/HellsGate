using HellsGate.Models;
using HellsGate.Models.DatabaseModel;
using HellsGate.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HellsGate.Controllers
{
    [Authorize]
    [Route("PlateVerification")]
    [ApiController]
    public class PlateVerificationController : ControllerBase
    {
        private readonly WellknownAuthorizationLevel AccessType = WellknownAuthorizationLevel.User;//TODO: add configuration reading
        private readonly IAccessManagerService _accessManager;

        public PlateVerificationController(IAccessManagerService accessManager)
        {
            _accessManager = accessManager ?? throw new ArgumentNullException(nameof(accessManager));
        }

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

            return await _accessManager.Access(newAccess, AccessType);
        }
    }
}