using HellsGate.Lib.Interfaces;
using HellsGate.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HellsGate.Controllers
{
    [Route("CardVerification")]
    [ApiController]
    public class CardVerificationController : ControllerBase
    {
        private readonly AuthType AccessType = AuthType.User;//TODO: add configuration reading
        private readonly IAccessManager AccessManager;

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

            return await AccessManager.Access(newAccess, AccessType); ;
        }
    }
}