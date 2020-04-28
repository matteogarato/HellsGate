using HellsGate.Models.DatabaseModel;
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

        [Route("")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateUser([FromBody] PeopleAnagraphicModel user, [FromBody] AutorizationLevelModel autorizationLevel)
        {
            try
            {
                if (user == null)
                {
                    return BadRequest();
                }
                if (autorizationLevel == null)
                {
                    return BadRequest();
                }
                _autorizationManagerService.CreateUser(user, autorizationLevel);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Route("Card")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateCard([FromBody] CardModel card)
        {
            try
            {
                if (card == null)
                {
                    return BadRequest();
                }
                _autorizationManagerService.CreateCard(card);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Route("Card")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateCard([FromBody]string cardNumber, [FromBody] DateTime newExpirationDate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    return BadRequest();
                }
                _autorizationManagerService.UpdateCardExpirationDate(cardNumber, newExpirationDate);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Route("{userId}/ChangeCardNumber")]
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateCard([FromQuery] Guid userId, [FromQuery] string cardNumber)
        {
            try
            {
                if (userId.Equals(Guid.Empty))
                {
                    return BadRequest();
                }
                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    return BadRequest();
                }
                _autorizationManagerService.ChangeCardNumber(userId, cardNumber);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}