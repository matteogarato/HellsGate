using HellsGate.Models;
using HellsGate.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HellsGate.Controllers
{
    [Authorize]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccessManagerService _accessManagerService;

        public AuthenticationController(IAccessManagerService accessManagerService)
        {
            _accessManagerService = accessManagerService ?? throw new ArgumentNullException(nameof(accessManagerService));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]AuthenticateModel model)
        {
            var user = _accessManagerService.ValidateLoginAsync(model.Username, model.Password, false, false);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
    }
}