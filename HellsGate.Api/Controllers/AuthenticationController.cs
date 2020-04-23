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
        private readonly IAutorizationManagerService _autorizationManagerService;

        public AuthenticationController(IAccessManagerService accessManagerService, IAutorizationManagerService autorizationManagerService)
        {
            _accessManagerService = accessManagerService ?? throw new ArgumentNullException(nameof(accessManagerService));
            _autorizationManagerService = autorizationManagerService ?? throw new ArgumentNullException(nameof(autorizationManagerService));
        }

        [Route("")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AuthenticateAsync([FromBody]AuthenticateModel model)
        {
            var user = await _accessManagerService.ValidateLoginAsync(model.Username, model.Password, false, false);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [Route("CreateAdmin")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateAdmin()
        {
            _autorizationManagerService.CreateAdmin();
            return Ok();
        }
    }
}