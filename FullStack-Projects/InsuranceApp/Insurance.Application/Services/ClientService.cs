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
            throw new ArgumentException("A client with the current CNP already exists.");
        }

        if (!string.IsNullOrWhiteSpace(dto.Cui) &&
            await _unitOfWork.Clients.ExistsByCuiAsync(dto.Cui))
        {
            throw new ArgumentException("A client with the current CUI already exists.");
        }

        var client = new Client
        {
            Id = Guid.NewGuid(),
            ClientTypeId = dto.ClientTypeId,
            Name = dto.Name,
            Cnp = dto.Cnp,
            Cui = dto.Cui,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address
        };

        await _unitOfWork.Clients.AddAsync(client);

        if (await _unitOfWork.SaveChangesAsync() == 0)
        {
            throw new InvalidOperationException("Failed to create client.");
        }

        return client.Id;
    }

    public async Task<GetClientByIdDto> GetClientByIdAsync(Guid clientId)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(clientId);
        if (client == null)
        {
            throw new NotFoundException("Client not found.");
        }

        var dto = _mapper.Map<GetClientByIdDto>(client);

        dto.ClientType = ((ClientType)client.ClientTypeId).ToString();

        dto.Identifier = client.Cnp ?? client.Cui ?? string.Empty;

        var buildings = await _unitOfWork.Buildings
            .GetBuildingsByClientIdAsync(clientId);

        dto.Buildings = _mapper.Map<IReadOnlyList<ClientBuildingSummaryDto>>(buildings);

        return dto;
    }

    public async Task<PagedClientsDto> SearchClientsAsync(
        string? name,
        string? identifier,
        int page,
        int pageSize)
    {
        var totalCount = await _unitOfWork.Clients.CountAsync(name, identifier);
        var clients = await _unitOfWork.Clients.SearchAsync(name, identifier, page, pageSize);

        var dtos = _mapper.Map<IReadOnlyList<GetClientsDto>>(clients);

        foreach (var dto in dtos)
        {
            var client = clients.First(c => c.Id == dto.Id);

            dto.ClientType = ((ClientType)client.ClientTypeId).ToString();

            dto.Identifier = client.Cnp ?? client.Cui ?? string.Empty;
        }

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
        var client = await _unitOfWork.Clients.GetByIdAsync(clientId);
        if (client == null)
        {
            throw new NotFoundException("Client not found.");
        }

        client.Name = dto.Name;
        client.Email = dto.Email;
        client.Phone = dto.Phone;
        client.Address = dto.Address;

        await _unitOfWork.Clients.UpdateAsync(client);

        if (await _unitOfWork.SaveChangesAsync() == 0)
        {
            throw new InvalidOperationException("Failed to update client.");
        }
    }

    public async Task UpdateClientIdentifierAsync(Guid clientId, UpdateClientIdentifierDto dto)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(clientId);
        if (client == null)
        {
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
        await _unitOfWork.SaveChangesAsync();
    }
}
