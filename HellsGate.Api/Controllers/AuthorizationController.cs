using HellsGate.Api.Models.Read;
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
        private readonly ILoginManagerService _loginManagerService;

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

        [Route("Plate")]
        [HttpGet]
        public async Task<IActionResult> VerifyPlateAsync([FromBody] string PlateNumber)
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

        [Route("UserLogin")]
        [HttpGet]
        public async Task<IActionResult> PasswordSignInAsync([FromBody] LoginModel loginModel)
        {
            if (loginModel == null)
            {
                return BadRequest();
            }
            var granted = await _loginManagerService.PasswordSignInAsync(loginModel.Username, loginModel.Password, loginModel.IsPersistent, loginModel.LockoutOnFailure);
            if (!granted.Succeeded)
            {
                return BadRequest();
            }
            return Ok(granted);
        }
    }
}