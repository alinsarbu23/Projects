using Insurance.Application.DTOs.Currencies;
using Insurance.Application.Interfaces.Services;
using Insurance.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers;

[ApiController]
[Route("api/admin/currencies")]
[Authorize(Roles = Roles.Admin)]
public class CurrenciesController : ControllerBase
{
    private readonly ICurrencyService _currencyService;

    public CurrenciesController(ICurrencyService currencyService)
    {
        _currencyService = currencyService;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromBody] CreateCurrencyDto dto)
    {
        return Ok(await _currencyService.CreateAsync(dto));
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
