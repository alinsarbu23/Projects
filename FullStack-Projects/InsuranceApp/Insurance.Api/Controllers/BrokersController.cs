using Insurance.Application.DTOs.Brokers;
using Insurance.Application.Interfaces.Services;
using Insurance.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers;

[ApiController]
[Route("api/admin/brokers")]
[Authorize(Roles = Roles.Admin)]
public class BrokersController : ControllerBase
{
    private readonly IBrokerService _brokerService;

    public BrokersController(IBrokerService brokerService)
    {
        _brokerService = brokerService;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateBrokerDto dto)
    {
        var brokerId = await _brokerService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { brokerId },
            null
        );
    }


    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetBrokersDto>>> GetAll()
    {
        return Ok(await _brokerService.GetAllAsync());
    }


    [HttpGet("{brokerId}")]
    public async Task<ActionResult<GetBrokerByIdDto>> GetById(Guid brokerId)
    {
        return Ok(await _brokerService.GetByIdAsync(brokerId));
    }

    [HttpPut("{brokerId}")]
    public async Task<IActionResult> Update(Guid brokerId, [FromBody] UpdateBrokerDto dto)
    {
        await _brokerService.UpdateAsync(brokerId, dto);
        return NoContent();
    }

    [HttpPost("{brokerId}/activate")]
    public async Task<IActionResult> Activate(Guid brokerId)
    {
        await _brokerService.ActivateAsync(brokerId);
        return NoContent();
    }

    [HttpPost("{brokerId}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid brokerId)
    {
        await _brokerService.DeactivateAsync(brokerId);
        return NoContent();
    }
}
