using Insurance.Application.DTOs.Fees;
using Insurance.Application.Interfaces.Services;
using Insurance.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers;

[ApiController]
[Route("api/admin/fees")]
[Authorize(Roles = Roles.Admin)]
public class FeeConfigurationsController : ControllerBase
{
    private readonly IFeeConfigurationService _feeService;

    public FeeConfigurationsController(IFeeConfigurationService feeService)
    {
        _feeService = feeService;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateFeeConfigurationDto dto)
    {
        return Ok(await _feeService.CreateAsync(dto));
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetFeeConfigurationsDto>>> GetAll()
    {
        return Ok(await _feeService.GetAllAsync());
    }

    [HttpGet("{feeId}")]
    public async Task<ActionResult<GetFeeConfigurationByIdDto>> GetById(Guid feeId)
    {
        return Ok(await _feeService.GetByIdAsync(feeId));
    }

    [HttpPut("{feeId}")]
    public async Task<IActionResult> Update(Guid feeId, [FromBody] UpdateFeeConfigurationDto dto)
    {
        await _feeService.UpdateAsync(feeId, dto);
        return NoContent();
    }
}
