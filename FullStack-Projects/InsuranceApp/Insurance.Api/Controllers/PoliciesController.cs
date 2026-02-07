using Insurance.Application.DTOs.Policies;
using Insurance.Application.Interfaces.Services;
using Insurance.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Insurance.Api.Controllers;

[ApiController]
[Route("api/brokers/policies")]
[Authorize(Roles = Roles.Broker)]
public class PoliciesController : ControllerBase
{
    private readonly IPolicyService _policyService;

    public PoliciesController(IPolicyService policyService)
    {
        _policyService = policyService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateDraft([FromBody] CreatePolicyDto dto)
    {
        var brokerEmail = User.FindFirstValue(ClaimTypes.Email)!;

        var policyId = await _policyService.CreateDraftAsync(dto, brokerEmail);

        return CreatedAtAction(
            nameof(GetById),
            new { policyId },
            null
        );
    }

    [HttpGet("{policyId}")]
    public async Task<ActionResult<GetPolicyByIdDto>> GetById(Guid policyId)
    {
        return Ok(await _policyService.GetByIdAsync(policyId));
    }

    [HttpGet]
    public async Task<ActionResult<PagedPoliciesDto>> Search(
        [FromQuery] Guid? clientId,
        [FromQuery] string? status,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var brokerEmail = User.FindFirstValue(ClaimTypes.Email)!;

        var result = await _policyService.SearchAsync(
            clientId,
            brokerEmail,
            status,
            startDate,
            endDate,
            page,
            pageSize);

        return Ok(result);
    }

    [HttpPost("{policyId}/activate")]
    public async Task<IActionResult> Activate(Guid policyId)
    {
        await _policyService.ActivateAsync(policyId);
        return NoContent();
    }

    [HttpPost("{policyId}/cancel")]
    public async Task<IActionResult> Cancel(Guid policyId, [FromBody] DeletePolicyDto dto)
    {
        await _policyService.CancelAsync(policyId, dto);
        return NoContent();
    }
}
