using HellsGate.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HellsGate.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IAutorizationManagerService _autorizationManagerService;

        public UserController(IAutorizationManagerService autorizationManagerService)
        {
            _autorizationManagerService = autorizationManagerService ?? throw new ArgumentNullException(nameof(autorizationManagerService));
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