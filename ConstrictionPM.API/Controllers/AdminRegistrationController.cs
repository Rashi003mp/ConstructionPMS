using Azure.Core;
using ConstructionPM.Application.Interfaces.Repositories.Queries;
using ConstructionPM.Application.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace ConstrictionPM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles="Admin")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationQueryRepository _query;
        private readonly IAdminApprovalService _service;

        public RegistrationController(
            IAdminApprovalService service,
            IRegistrationQueryRepository query)
        {
            _query = query;
            _service = service;
        }

        [HttpGet("requests/pending")]
        public async Task<IActionResult> GetPending()
        {
            var data = await _query.GetPendingAsync();
            return Ok(data);
        }

        [HttpPost("requests/{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            await _service.ApproveAsync(id);
            return Ok("Registration approved");
        }

        [HttpPost("requests/{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            await _service.RejectAsync(id);
            return Ok("Registration rejected");
        }



    }
}
