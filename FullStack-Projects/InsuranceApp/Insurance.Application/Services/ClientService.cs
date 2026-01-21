using AutoMapper;
using Insurance.Application.DTOs.Clients;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;

namespace Insurance.Application.Services;

public class ClientService : IClientService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    public ClientService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreateClientAsync(CreateClientDto clientDto)
    {
        if(clientDto.ClientType == ClientType.Individual && string.IsNullOrEmpty(clientDto.Cnp))
        {
            throw new ArgumentException("CNP is required for individual clients.");
        }

        if(clientDto.ClientType == ClientType.Company && string.IsNullOrEmpty(clientDto.Cui))
        {
            throw new ArgumentException("CUI is required for company clients.");
        }

        if(!string.IsNullOrWhiteSpace(clientDto.Cnp) && await _unitOfWork.Clients.ExistsByCnpAsync(clientDto.Cnp))
        {
            throw new ArgumentException("A client with the current CNP already exists.");
        }

        if(!string.IsNullOrWhiteSpace(clientDto.Cui) && await _unitOfWork.Clients.ExistsByCuiAsync(clientDto.Cui))
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
        var rowsAffected =  await _unitOfWork.SaveChangesAsync();

        if(rowsAffected == 0)
        {
            throw new InvalidOperationException("Failed to create client.");
        }

        return client.Id;

    }

    public async Task<GetClientByIdDto> GetClientByIdAsync(Guid clientId)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(clientId)
            ?? throw new KeyNotFoundException("Client not found.");

        var dto = _mapper.Map<GetClientByIdDto>(client);

        
        dto.ClientType = client.ClientType.ToString();
        if (client.ClientType == ClientType.Individual)
        {
            dto.Identifier = client.Cnp;
        }
        else
        {
            dto.Identifier = client.Cui;
        }

        var buildings = await _unitOfWork.Buildings.GetBuildingsByClientIdAsync(clientId);
        dto.Buildings = _mapper.Map<IReadOnlyList<ClientBuildingSummaryDto>>(buildings);

        return dto;
    }

    public async Task<IReadOnlyList<GetClientsDto>> SearchClientsAsync(string? name, string? identifier, int page, int pageSize)
    {
        var clients = await _unitOfWork.Clients.SearchAsync(name, identifier, page, pageSize);
        if (clients == null || !clients.Any()) 
        {
            throw new KeyNotFoundException("No clients found matching the criteria.");
        }

        var clientsDto = _mapper.Map<IReadOnlyList<GetClientsDto>>(clients);
        foreach(var clientDto in clientsDto)
        {
            var client = clients.First(client => client.Id == clientDto.Id);
            clientDto.ClientType = client.ClientType.ToString();

            if (client.ClientType == ClientType.Individual)
            {
                clientDto.Identifier = client.Cnp;
            }
            else
            {
                clientDto.Identifier = client.Cui;
            }
        }

        return clientsDto;

    }

    public async Task UpdateClientAsync(Guid clientId, UpdateClientDto clientDto)
    {
        var existingClient = await _unitOfWork.Clients.GetByIdAsync(clientId);
        
        if(existingClient == null)
        {
            throw new KeyNotFoundException("Client not found.");
        }

        existingClient.Email = clientDto.Email;
        existingClient.Phone = clientDto.Phone;
        existingClient.Address = clientDto.Address; 

        await _unitOfWork.Clients.UpdateAsync(existingClient);

        var rowsAffected = await _unitOfWork.SaveChangesAsync();

        if(rowsAffected == 0)
        {
            throw new InvalidOperationException("Failed to update client.");
        }
    }

    public async Task UpdateClientIdentifierAsync(Guid clientId, UpdateClientIdentifierDto updateClientIdentifierDto)
    {
        var client = await _unitOfWork.Clients.GetByIdAsync(clientId);
        if(client == null)
        {
            throw new KeyNotFoundException("Client not found.");
        }

        if(client.ClientType == ClientType.Individual)
        {
            if(await _unitOfWork.Clients.ExistsByCnpAsync(updateClientIdentifierDto.NewIdentifier))
            {
                throw new ArgumentException("A client with the new CNP already exists.");
            }
            client.Cnp = updateClientIdentifierDto.NewIdentifier;
        }
        else
        {
            if(await _unitOfWork.Clients.ExistsByCuiAsync(updateClientIdentifierDto.NewIdentifier))
            {
                throw new ArgumentException("A client with the new CUI already exists.");
            }
            client.Cui = updateClientIdentifierDto.NewIdentifier;
        }

        await _unitOfWork.Clients.UpdateAsync(client);
        var rowsAffected = await _unitOfWork.SaveChangesAsync();
        if(rowsAffected == 0)
        {
            throw new InvalidOperationException("Failed to update client identifier.");
        }

    }
}
