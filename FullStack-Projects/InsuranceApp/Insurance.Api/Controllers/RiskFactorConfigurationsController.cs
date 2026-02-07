using Insurance.Application.DTOs.RiskFactors;
using Insurance.Application.Interfaces.Services;
using Insurance.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers;

[ApiController]
[Route("api/admin/risk-factors")]
[Authorize(Roles = Roles.Admin)]
public class RiskFactorConfigurationsController : ControllerBase
{
    private readonly IRiskFactorConfigurationService _riskService;

    public RiskFactorConfigurationsController(IRiskFactorConfigurationService riskService)
    {
        _riskService = riskService;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateRiskFactorConfigurationDto dto)
    {
        return Ok(await _riskService.CreateAsync(dto));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetRiskFactorConfigurationsDto>>> GetAll()
    {
        return Ok(await _riskService.GetAllAsync());
    }

    [HttpGet("{riskId}")]
    public async Task<ActionResult<GetRiskFactorConfigurationByIdDto>> GetById(Guid riskId)
    {
        return Ok(await _riskService.GetByIdAsync(riskId));
    }

    [HttpPut("{riskId}")]
    public async Task<IActionResult> Update(Guid riskId, [FromBody] UpdateRiskFactorConfigurationDto dto)
    {
        await _riskService.UpdateAsync(riskId, dto);
        return NoContent();
    }
}
