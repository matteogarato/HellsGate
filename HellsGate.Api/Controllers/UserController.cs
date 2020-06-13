using HellsGate.Api.Models.Create;
using HellsGate.Models;
using HellsGate.Models.DatabaseModel;
using HellsGate.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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

        [Route("ChangeCardNumber")]
        [HttpPost]
        public async Task<IActionResult> ChangeCardNumber([FromQuery] Guid userId, [FromQuery] string cardNumber)
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
                var created = await _autorizationManagerService.ChangeCardNumber(userId, cardNumber);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Route("CreateAdmin")]
        [HttpPost]
        public IActionResult CreateAdmin()
        {
            _autorizationManagerService.CreateAdmin();
            return Ok();
        }

        [Route("Card")]
        [HttpPost]
        public async Task<IActionResult> CreateCard([FromBody] CardModel card)
        {
            try
            {
                if (card == null)
                {
                    return BadRequest();
                }
                var created = await _autorizationManagerService.CreateCard(card);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Route("")]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel createModel)
        {
            try
            {
                if (createModel == null)
                {
                    return BadRequest();
                }
                var usr = new PeopleAnagraphicModel()
                {
                    Email = createModel.Email,
                    Name = createModel.Name,
                    Surname = createModel.Surname
                };
                var auth = new AutorizationLevelModel()
                {
                    AuthName = createModel.AuthName,
                    AuthValue = (WellknownAuthorizationLevel)createModel.AuthValue
                };
                var created = await _autorizationManagerService.CreateUser(usr, auth);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Route("Card")]
        [HttpPut]
        public async Task<IActionResult> UpdateCard([FromQuery] string cardNumber, [FromQuery] DateTime newExpirationDate)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    return BadRequest();
                }
                var created = await _autorizationManagerService.UpdateCardExpirationDate(cardNumber, newExpirationDate);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}