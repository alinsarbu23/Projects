using Insurance.Application.DTOs.Clients;
using Insurance.Application.Interfaces.Services;
using Insurance.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Insurance.Api.Controllers;

[ApiController]
[Route("api/brokers/clients")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] CreateClientDto dto)
    {
        var clientId = await _clientService.CreateClientAsync(dto);
        return CreatedAtAction(nameof(GetClientById), new { clientId }, null);
    }

    [HttpPut("{clientId:guid}")]
    public async Task<IActionResult> UpdateClient(Guid clientId, [FromBody] UpdateClientDto dto)
    {
        await _clientService.UpdateClientAsync(clientId, dto);
        return NoContent();
    }

    [HttpPut("{clientId:guid}/identifier")]
    public async Task<IActionResult> UpdateClientIdentifier(Guid clientId, [FromBody] UpdateClientIdentifierDto dto)
    {
        await _clientService.UpdateClientIdentifierAsync(clientId, dto);
        return NoContent();
    }

    [HttpGet("{clientId:guid}")]
    public async Task<ActionResult<GetClientByIdDto>> GetClientById(Guid clientId)
    {
        var client = await _clientService.GetClientByIdAsync(clientId);
        return Ok(client);
    }

    /// <summary>
    /// Search clients by name (partial) or identifier (exact), with paging.
    /// Validation is handled via FluentValidation (to be added).
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<PagedClientsDto>> SearchClients(
        [FromQuery] string? name,
        [FromQuery] string? identifier,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await _clientService
            .SearchClientsAsync(name, identifier, page, pageSize);

        return Ok(result);
    }
}
