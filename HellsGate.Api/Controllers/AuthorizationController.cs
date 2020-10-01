using HellsGate.Api.Models.Read;
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
    [Route("api/Authorization")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAccessManagerService _accessManager;

        public AuthorizationController(IAccessManagerService accessManager)
        {
            _accessManager = accessManager ?? throw new ArgumentNullException(nameof(accessManager));
        }

        [Route("Card")]
        [HttpGet]
        public async Task<IActionResult> VerifyCardAsync([FromBody] AccessReadModel CardReaded)
        {
            if (CardReaded == null)
            {
                return BadRequest();
            }
            var newAccess = new AccessModel
            {
                AccessTime = DateTime.UtcNow,
                GrantedAccess = false,
                CardNumber = CardReaded.CardNumber,
                MacAddress = CardReaded.MacAddress,
                NodeName = CardReaded.NodeName
            };

            var granted = await _accessManager.Access(newAccess);
            if (!granted)
            {
                return BadRequest();
            }
            return Ok(granted);
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

            var granted = await _accessManager.Access(newAccess);
            if (!granted)
            {
                return BadRequest();
            }
            return Ok(granted);
        }
    }
}