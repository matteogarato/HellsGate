using HellsGate.Models.DatabaseModel;
using HellsGate.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HellsGate.Controllers
{
    [Route("api/NodeController")]
    [ApiController]
    public class NodeController : ControllerBase
    {
        private readonly INodeService _nodeService;

        public NodeController(INodeService nodeService)
        {
            _nodeService = nodeService ?? throw new ArgumentNullException(nameof(nodeService));
        }

        [Route("{Id}")]
        [AllowAnonymous]
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
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateAsync([FromBody] NodeCreateModel model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var node = _nodeService.Create(model);
            if (node == null)
            {
                return NotFound();
            }
            return Ok(node);
        }

        [Route("")]
        [AllowAnonymous]
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

        [Route("")]
        [AllowAnonymous]
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
    }
}