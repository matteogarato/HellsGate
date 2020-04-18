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
    [Route("CardVerification")]
    [ApiController]
    public class CardVerificationController : ControllerBase
    {
        private readonly WellknownAuthorizationLevel AccessType = WellknownAuthorizationLevel.User;//TODO: add configuration reading
        private readonly IAccessManagerService _accessManager;

        public CardVerificationController(IAccessManagerService accessManager)
        {
            _accessManager = accessManager ?? throw new ArgumentNullException(nameof(accessManager));
        }

        [HttpGet("{CardId}")]
        public async Task<bool> Get(string CardId)
        {
            if (string.IsNullOrEmpty(CardId) || string.IsNullOrEmpty(CardId.Trim()))
            {
                return false;
            }
            var newAccess = new AccessModel
            {
                AccessTime = DateTime.Now,
                GrantedAccess = false,
                CardNumber = CardId
            };

            return await _accessManager.Access(newAccess, AccessType); ;
        }
    }
}