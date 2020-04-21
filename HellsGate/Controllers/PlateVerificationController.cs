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
    [Route("api/PlateVerification")]
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
        public async Task<IActionResult> GetAsync(string PlateNumber)
        {
            if (string.IsNullOrWhiteSpace(PlateNumber))
            {
                return BadRequest();
            }
            var newAccess = new AccessModel
            {
                AccessTime = DateTime.Now,
                GrantedAccess = false,
                Plate = PlateNumber
            };

            var granted = await _accessManager.Access(newAccess, AccessType);
            if (!granted)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}