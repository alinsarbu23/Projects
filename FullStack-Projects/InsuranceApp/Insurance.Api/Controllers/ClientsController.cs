using Insurance.Application.DTOs.Clients;
using Insurance.Application.Interfaces.Services;
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

    [HttpPost("individuals")]
    public async Task<IActionResult> CreateIndividual([FromBody] CreateIndividualClientDto dto)
    {
        var clientId = await _clientService.CreateIndividualClientAsync(dto);
        return CreatedAtAction(nameof(GetClientById), new { clientId }, null);
    }

    [HttpPost("companies")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyClientDto dto)
    {
        var clientId = await _clientService.CreateCompanyClientAsync(dto);
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


    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<GetClientsDto>>> SearchClients([FromQuery] string? name, [FromQuery] string? identifier)
    {
        var clients = await _clientService.SearchClientsAsync(name, identifier);
        return Ok(clients);
    }
}
