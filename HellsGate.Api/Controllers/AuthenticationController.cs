using HellsGate.Models;
using HellsGate.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HellsGate.Controllers
{
    //[Authorize]
    [Route("api/Authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAccessManagerService _accessManagerService;

        public AuthenticationController(IAccessManagerService accessManagerService)
        {
            _accessManagerService = accessManagerService ?? throw new ArgumentNullException(nameof(accessManagerService));
        }

        [Route("Authenticate")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync([FromBody]AuthenticateModel model)
        {
            var user = await _accessManagerService.ValidateLoginAsync(model.Username, model.Password, false, false);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }
    }
}