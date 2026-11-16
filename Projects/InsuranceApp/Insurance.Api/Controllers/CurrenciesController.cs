using Insurance.Application.DTOs.Currencies;
using Insurance.Application.Interfaces.Services;
using Insurance.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers;

[ApiController]
[Route("api/currencies")]
[Authorize(Roles = $"{Roles.Admin},{Roles.Broker}")]
public class CurrenciesController : ControllerBase
{
    private readonly ICurrencyService _currencyService;

    public CurrenciesController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCurrencyDto dto)
    {
        var currencyId = await _currencyService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { currencyId },
            null
        );
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetCurrenciesDto>>> GetAll()
    {
        return Ok(await _currencyService.GetAllAsync());
    }

    [HttpGet("{currencyId}")]
    public async Task<ActionResult<GetCurrencyByIdDto>> GetById(Guid currencyId)
    {
        return Ok(await _currencyService.GetByIdAsync(currencyId));
    }

    [HttpPut("{currencyId}")]
    public async Task<IActionResult> Update(Guid currencyId, [FromBody] UpdateCurrencyDto dto)
    {
        await _currencyService.UpdateAsync(currencyId, dto);
        return NoContent();
    }
}
