using AutoMapper;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.Exceptions;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Domain.Models;
using Microsoft.Extensions.Logging;
using ClientType = Insurance.Domain.Enums.ClientType;

namespace Insurance.Application.Services;

public class ClientService : IClientService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ClientService> _logger;

    public ClientService(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        ILogger<ClientService> logger)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Guid> CreateClientAsync(CreateClientDto dto)
    {
        _logger.LogInformation(
            "Creating client {Name} (ClientTypeId={ClientTypeId})",
            dto.Name,
            dto.ClientTypeId);

        if (!string.IsNullOrWhiteSpace(dto.Cnp) &&
            await _unitOfWork.Clients.ExistsByCnpAsync(dto.Cnp))
        {
            _logger.LogWarning("Client creation failed. Duplicate CNP {Cnp}", dto.Cnp);
            throw new ArgumentException("A client with the current CNP already exists.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Cui) &&
            await _unitOfWork.Clients.ExistsByCuiAsync(dto.Cui))
        {
            _logger.LogWarning("Client creation failed. Duplicate CUI {Cui}", dto.Cui);
            throw new ArgumentException("A client with the current CUI already exists.");
        }

        var client = _mapper.Map<Client>(dto);
        client.Id = Guid.NewGuid();

        await _unitOfWork.Clients.AddAsync(client);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError("Critical error while saving client {Name}", dto.Name);
            throw new InvalidOperationException("Failed to create client.");
        }

        _logger.LogInformation("Client {ClientId} created successfully", client.Id);

        return client.Id;
    }

    public async Task<GetClientByIdDto> GetClientByIdAsync(Guid clientId)
    {
        _logger.LogInformation("Retrieving client {ClientId}", clientId);

        var client = await _unitOfWork.Clients.GetByIdAsync(clientId);

        if (client == null)
        {
            _logger.LogWarning("Client {ClientId} not found", clientId);
            throw new NotFoundException("Client not found.");
        }

        var dto = _mapper.Map<GetClientByIdDto>(client);

        dto.ClientType = ((ClientType)client.ClientTypeId).ToString();
        dto.Identifier = client.Cnp ?? client.Cui ?? string.Empty;

        var buildings = await _unitOfWork.Buildings
            .GetBuildingsByClientIdAsync(clientId);

        dto.Buildings = _mapper.Map<IReadOnlyList<ClientBuildingSummaryDto>>(buildings);

        _logger.LogInformation("Client {ClientId} retrieved successfully", clientId);

        return dto;
    }

    public async Task<PagedClientsDto> SearchClientsAsync(
        string? name,
        string? identifier,
        int page,
        int pageSize)
    {
        _logger.LogInformation(
            "Searching clients (Name={Name}, Identifier={Identifier}, Page={Page}, PageSize={PageSize})",
            name,
            identifier,
            page,
            pageSize);

        var totalCount = await _unitOfWork.Clients.CountAsync(name, identifier);
        var clients = await _unitOfWork.Clients.SearchAsync(name, identifier, page, pageSize);

        var dtos = _mapper.Map<IReadOnlyList<GetClientsDto>>(clients);

        foreach (var dto in dtos)
        {
            var client = clients.First(c => c.Id == dto.Id);

            dto.ClientType = ((ClientType)client.ClientTypeId).ToString();
            dto.Identifier = client.Cnp ?? client.Cui ?? string.Empty;
        }

        _logger.LogInformation(
            "Search completed. Found {TotalCount} clients, returning {ReturnedCount}",
            totalCount,
            dtos.Count);

        return new PagedClientsDto
        {
            Items = dtos,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task UpdateClientAsync(Guid clientId, UpdateClientDto dto)
    {
        _logger.LogInformation("Updating client {ClientId}", clientId);

        var client = await _unitOfWork.Clients.GetByIdAsync(clientId);

        if (client == null)
        {
            _logger.LogWarning("Update failed. Client {ClientId} not found", clientId);
            throw new NotFoundException("Client not found.");
        }

        _mapper.Map(dto, client);

        await _unitOfWork.Clients.UpdateAsync(client);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError("Critical error while updating client {ClientId}", clientId);
            throw new InvalidOperationException("Failed to update client.");
        }

        _logger.LogInformation("Client {ClientId} updated successfully", clientId);
    }

    public async Task UpdateClientIdentifierAsync(Guid clientId, UpdateClientIdentifierDto dto)
    {
        _logger.LogInformation("Updating identifier for client {ClientId}", clientId);

        var client = await _unitOfWork.Clients.GetByIdAsync(clientId);

        if (client == null)
        {
            _logger.LogWarning("Identifier update failed. Client {ClientId} not found", clientId);
            throw new NotFoundException("Client not found.");
        }

        var oldIdentifier = client.Cnp ?? client.Cui;

        if (!string.IsNullOrWhiteSpace(client.Cnp))
        {
            client.Cnp = dto.NewIdentifier;
        }
        else
        {
            client.Cui = dto.NewIdentifier;
        }

        var audit = new ClientIdentifier
        {
            Id = Guid.NewGuid(),
            ClientId = client.Id,
            OldIdentifier = oldIdentifier!,
            NewIdentifier = dto.NewIdentifier,
            IdentifierType = "CLIENT",
            ChangedAt = DateTime.UtcNow
        };

        await _unitOfWork.ClientIdentifiers.AddAsync(audit);
        await _unitOfWork.Clients.UpdateAsync(client);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError("Critical error while updating identifier for client {ClientId}", clientId);
            throw new InvalidOperationException("Failed to update identifier.");
        }

        _logger.LogInformation("Client {ClientId} identifier updated successfully", clientId);
    }
}
