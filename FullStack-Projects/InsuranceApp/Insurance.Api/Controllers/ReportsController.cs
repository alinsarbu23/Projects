using Insurance.Application.DTOs.Reports;
using Insurance.Application.Interfaces.Services;
using Insurance.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers;

[ApiController]
[Route("api/reports")]
[Authorize(Roles = Roles.Admin)]
public class ReportsController : ControllerBase
{
    private readonly IReportsService _reportsService;

    public ReportsController(IReportsService reportsService)
    {
        _reportsService = reportsService;
    }

    [HttpGet("policies-by-country")]
    public async Task<ActionResult<IReadOnlyList<GetPoliciesByCountryReportDto>>> GetPoliciesByCountryAsync([FromQuery] GetPoliciesReportDto dto)
    {
        var result = await _reportsService.GetPoliciesByCountryAsync(dto);
        return Ok(result);
    }

    [HttpGet("policies-by-county")]
    public async Task<ActionResult<IReadOnlyList<GetPoliciesByCountyReportDto>>> GetPoliciesByCountyAsync([FromQuery] GetPoliciesReportDto dto)
    {
        var result = await _reportsService.GetPoliciesByCountyAsync(dto);
        return Ok(result);
    }

    [HttpGet("policies-by-city")]
    public async Task<ActionResult<IReadOnlyList<GetPoliciesByCityReportDto>>> GetPoliciesByCityAsync([FromQuery] GetPoliciesReportDto dto)
    {
        var result = await _reportsService.GetPoliciesByCityAsync(dto);
        return Ok(result);
    }

    [HttpGet("policies-by-broker")]
    public async Task<ActionResult<IReadOnlyList<GetPoliciesByBrokerReportDto>>> GetPoliciesByBrokerAsync([FromQuery] GetPoliciesReportDto dto)
    {
        var result = await _reportsService.GetPoliciesByBrokerAsync(dto);
        return Ok(result);
    }
}
