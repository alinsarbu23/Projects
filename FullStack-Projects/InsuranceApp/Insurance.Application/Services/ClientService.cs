using AutoMapper;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;
using Microsoft.Extensions.Logging;

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

    public async Task<Guid> CreateClientAsync(CreateClientDto clientDto)
    {
        _logger.LogInformation(
            "Creating client {Name} ({Type})",
            clientDto.Name,
            clientDto.ClientType);

        if (!string.IsNullOrWhiteSpace(clientDto.Cnp) && await _unitOfWork.Clients.ExistsByCnpAsync(clientDto.Cnp))
        {
            throw new ArgumentException("A client with the current CNP already exists.");
        }

        if (!string.IsNullOrWhiteSpace(clientDto.Cui) && await _unitOfWork.Clients.ExistsByCuiAsync(clientDto.Cui))
        {
            throw new ArgumentException("A client with the current CUI already exists.");
        }

        var client = new Client
        {
            Id = Guid.NewGuid(),
            ClientType = clientDto.ClientType,
            Name = clientDto.Name,
            Cnp = clientDto.Cnp,
            Cui = clientDto.Cui,
            Email = clientDto.Email,
            Phone = clientDto.Phone,
            Address = clientDto.Address
        };

        await _unitOfWork.Clients.AddAsync(client);
        var rowsAffected = await _unitOfWork.SaveChangesAsync();

        if (rowsAffected == 0)
        {
            _logger.LogWarning("Failed to create client {Name}", clientDto.Name);
            throw new InvalidOperationException("Failed to create client.");
        }

        _logger.LogInformation(
            "Client created successfully with id {ClientId}",
            client.Id);

        return client.Id;
    }

    public async Task<GetClientByIdDto> GetClientByIdAsync(Guid clientId)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(clientId);
        if(client == null)
        {
            throw new KeyNotFoundException("Client not found.");
        }

        var dto = _mapper.Map<GetClientByIdDto>(client);

        dto.ClientType = client.ClientType.ToString();
        dto.Identifier = client.ClientType == ClientType.Individual
            ? client.Cnp
            : client.Cui;

        var buildings = await _unitOfWork.Buildings.GetBuildingsByClientIdAsync(clientId);

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

        var clientsDto = _mapper.Map<IReadOnlyList<GetClientsDto>>(clients);

        foreach (var dto in clientsDto)
        {
            var client = clients.First(c => c.Id == dto.Id);
            dto.ClientType = client.ClientType.ToString();
            dto.Identifier = client.ClientType == ClientType.Individual
                ? client.Cnp
                : client.Cui;
        }

        return new PagedClientsDto
        {
            Items = clientsDto,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task UpdateClientAsync(Guid clientId, UpdateClientDto dto)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(clientId);
        if(client == null)
        {
            throw new KeyNotFoundException("Client not found.");
        }

        client.Email = dto.Email;
        client.Phone = dto.Phone;
        client.Address = dto.Address;

        await _unitOfWork.Clients.UpdateAsync(client);

        if (await _unitOfWork.SaveChangesAsync() == 0)
        {
            throw new InvalidOperationException("Failed to update client.");
        } 
    }

    public async Task UpdateClientIdentifierAsync(Guid clientId, UpdateClientIdentifierDto updateClientDto)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(clientId);

        if(client == null)
        {
            throw new KeyNotFoundException("Client not found.");
        }

        var lastChangeDate = await _unitOfWork.ClientIdentifiers.GetLastChangeDateAsync(clientId);

        if (lastChangeDate.HasValue && DateTime.UtcNow < lastChangeDate.Value.AddMonths(1))
        {
            throw new InvalidOperationException("Client identifier can only be changed once per month.");
        }
            
        var oldIdentifier = client.ClientType == ClientType.Individual ? client.Cnp : client.Cui;

        if (client.ClientType == ClientType.Individual)
        {
            client.Cnp = updateClientDto.NewIdentifier;
        }   
        else
        {
            client.Cui = updateClientDto.NewIdentifier;
        }
            

        var audit = new ClientIdentifier
        {
            Id = Guid.NewGuid(),
            ClientId = client.Id,
            OldIdentifier = oldIdentifier!,
            NewIdentifier = updateClientDto.NewIdentifier,
            IdentifierType = client.ClientType.ToString(),
            ChangedAt = DateTime.UtcNow
        };

        await _unitOfWork.ClientIdentifiers.AddAsync(audit);
        await _unitOfWork.Clients.UpdateAsync(client);
        await _unitOfWork.SaveChangesAsync();
    }
}
