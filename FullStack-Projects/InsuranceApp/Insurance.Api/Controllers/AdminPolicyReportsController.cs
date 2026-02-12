using Insurance.Application.DTOs.Policies;
using Insurance.Application.Interfaces.Services;
using Insurance.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers;

[ApiController]
[Route("api/policyReports")]
[Authorize(Roles = Roles.Admin)]
public class AdminPolicyReportsController : ControllerBase
{
    private readonly IPolicyService _policyService;

    public AdminPolicyReportsController(IPolicyService policyService)
    {
        _policyService = policyService;
    }

    [HttpGet("by-broker")]
    public async Task<ActionResult<IReadOnlyList<PolicyReportDto>>> GetPolicyReportByBroker(
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var result = await _policyService.GetPolicyReportByBrokerAsync(from, to);
        return Ok(result);
    }
}
