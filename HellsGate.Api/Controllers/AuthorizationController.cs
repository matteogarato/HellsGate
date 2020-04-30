using HellsGate.Models;
using HellsGate.Models.DatabaseModel;
using HellsGate.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HellsGate.Controllers
{
    //[Authorize]
    [Route("Authorization")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAccessManagerService _accessManager;
        private readonly WellknownAuthorizationLevel AccessType = WellknownAuthorizationLevel.User;//TODO: add configuration reading

        public AuthorizationController(IAccessManagerService accessManager)
        {
            _accessManager = accessManager ?? throw new ArgumentNullException(nameof(accessManager));
        }

        [Route("Card/{CardId}")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> VerifyCardAsync(string CardId)
        {
            if (string.IsNullOrWhiteSpace(CardId))
            {
                return BadRequest();
            }
            var newAccess = new AccessModel
            {
                AccessTime = DateTime.UtcNow,
                GrantedAccess = false,
                CardNumber = CardId
            };

            var granted = await _accessManager.Access(newAccess, AccessType);
            if (!granted)
            {
                return BadRequest();
            }
            return Ok();
        }

        [Route("Plate/{PlateNumber}")]
        [HttpGet]
        public async Task<IActionResult> VerifyPlateAsync(string PlateNumber)
        {
            if (string.IsNullOrWhiteSpace(PlateNumber))
            {
                return BadRequest();
            }
            var newAccess = new AccessModel
            {
                AccessTime = DateTime.UtcNow,
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