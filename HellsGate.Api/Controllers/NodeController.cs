using HellsGate.Models.DatabaseModel;
using HellsGate.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HellsGate.Controllers
{
    [Authorize]
    [Route("api/NodeController")]
    [ApiController]
    public class NodeController : ControllerBase
    {
        private readonly INodeService _nodeService;

        public NodeController(INodeService nodeService)
        {
            _nodeService = nodeService ?? throw new ArgumentNullException(nameof(nodeService));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] Api.Models.Read.AuthenticateModel model)
        {
            var user = await _nodeService.Authenticate(model.NodeName, model.MacAddress);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [Route("")]
        [HttpPost]
        public IActionResult CreateAsync([FromBody] NodeCreateModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var node = _nodeService.Create(model);
            if (node != Guid.Empty)
            {
                return NotFound();
            }
            return Ok(node);
        }

        [Route("")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid Id)
        {
            if (Id.Equals(Guid.Empty))
            {
                return BadRequest();
            }
            var res = await _nodeService.DeleteAsync(Id);
            if (!res)
            {
                return NotFound();
            }
            return Ok();
        }

        [Route("{Id}")]
        [HttpGet]
        public async Task<IActionResult> GetAsync(Guid Id)
        {
            if (Id.Equals(Guid.Empty))
            {
                return BadRequest();
            }
            var node = await _nodeService.GetByIdAsync(Id);
            if (node == null)
            {
                return NotFound();
            }
            return Ok(node);
        }

        [Route("")]
        [HttpPut]
        public async Task<IActionResult> UpdateAsync([FromBody] NodeUpdateModel model)
        {
            if (model == null || model.Id.Equals(Guid.Empty))
            {
                return BadRequest();
            }
            var res = await _nodeService.UpdateAsync(model);
            if (!res)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}