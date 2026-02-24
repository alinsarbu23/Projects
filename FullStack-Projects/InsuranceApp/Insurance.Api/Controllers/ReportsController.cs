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

    [HttpGet("policies-by-broker")]
    public async Task<ActionResult<PagedResultDto<GetPoliciesByBrokerReportDto>>> GetPoliciesByBroker(
            [FromQuery] GetPoliciesReportDto dto,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
    {
        var result = await _reportsService.GetPoliciesByBrokerAsync(dto, page, pageSize);

        return Ok(result);
    }

    [HttpGet("policies-by-country")]
    public async Task<ActionResult<PagedResultDto<GetPoliciesByCountryReportDto>>> GetPoliciesByCountry(
            [FromQuery] GetPoliciesReportDto dto,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
    {
        var result = await _reportsService.GetPoliciesByCountryAsync(dto, page, pageSize);

        return Ok(result);
    }

    [HttpGet("policies-by-county")]
    public async Task<ActionResult<PagedResultDto<GetPoliciesByCountyReportDto>>> GetPoliciesByCounty(
            [FromQuery] GetPoliciesReportDto dto,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
    {
        var result = await _reportsService.GetPoliciesByCountyAsync(dto, page, pageSize);

        return Ok(result);
    }

    [HttpGet("policies-by-city")]
    public async Task<ActionResult<PagedResultDto<GetPoliciesByCityReportDto>>> GetPoliciesByCity(
            [FromQuery] GetPoliciesReportDto dto,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
    {
        var result = await _reportsService.GetPoliciesByCityAsync(dto, page, pageSize);

        return Ok(result);
    }
}
