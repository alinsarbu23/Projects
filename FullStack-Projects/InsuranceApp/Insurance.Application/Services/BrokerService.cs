using AutoMapper;
using Insurance.Application.DTOs.Brokers;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Insurance.Domain.Enums;
using Insurance.Domain.Models;
using Microsoft.Extensions.Logging;

namespace Insurance.Application.Services;

public class BrokerService : IBrokerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<BrokerService> _logger;

    public BrokerService(IUnitOfWork uow, IMapper mapper, ILogger<BrokerService> logger)
    {
        _unitOfWork = uow;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Guid> CreateAsync(CreateBrokerDto dto)
    {
        _logger.LogInformation("Creating broker with code {BrokerCode}", dto.BrokerCode);

        if (await _unitOfWork.Brokers.GetByCodeAsync(dto.BrokerCode) != null)
        {
            _logger.LogWarning("Broker creation failed. Duplicate BrokerCode {BrokerCode}", dto.BrokerCode);
            throw new ArgumentException("Broker code must be unique");
        }

        if (await _unitOfWork.Brokers.GetByEmailAsync(dto.Email) != null)
        {
            _logger.LogWarning("Broker creation failed. Duplicate Email {Email}", dto.Email);
            throw new ArgumentException("Broker email must be unique");
        }

        var broker = _mapper.Map<Broker>(dto);
        broker.Id = Guid.NewGuid();
        broker.Status = BrokerStatus.Inactive;

        await _unitOfWork.Brokers.AddAsync(broker);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError("Critical error while saving broker {BrokerCode}", dto.BrokerCode);
            throw new InvalidOperationException("Failed to create broker.");
        }

        _logger.LogInformation("Broker {BrokerId} created successfully", broker.Id);

        return broker.Id;
    }

    public async Task UpdateAsync(Guid brokerId, UpdateBrokerDto dto)
    {
        _logger.LogInformation("Updating broker {BrokerId}", brokerId);

        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);

        if (broker == null)
        {
            _logger.LogWarning("Update failed. Broker {BrokerId} not found", brokerId);
            throw new InvalidOperationException("Broker not found");
        }

        _mapper.Map(dto, broker);

        await _unitOfWork.Brokers.UpdateAsync(broker);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError("Critical error while updating broker {BrokerId}", brokerId);
            throw new InvalidOperationException("Failed to update broker.");
        }

        _logger.LogInformation("Broker {BrokerId} updated successfully", brokerId);
    }

    public async Task ActivateAsync(Guid brokerId)
    {
        _logger.LogInformation("Activating broker {BrokerId}", brokerId);

        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);

        if (broker == null)
        {
            _logger.LogWarning("Activation failed. Broker {BrokerId} not found", brokerId);
            throw new InvalidOperationException("Broker not found");
        }

        if (broker.Status == BrokerStatus.Active)
        {
            _logger.LogWarning("Broker {BrokerId} is already active", brokerId);
            throw new InvalidOperationException("Broker is already active.");
        }

        broker.Status = BrokerStatus.Active;

        await _unitOfWork.Brokers.UpdateAsync(broker);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError("Critical error while activating broker {BrokerId}", brokerId);
            throw new InvalidOperationException("Failed to activate broker.");
        }

        _logger.LogInformation("Broker {BrokerId} activated successfully", brokerId);
    }

    public async Task DeactivateAsync(Guid brokerId)
    {
        _logger.LogInformation("Deactivating broker {BrokerId}", brokerId);

        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);

        if (broker == null)
        {
            _logger.LogWarning("Deactivation failed. Broker {BrokerId} not found", brokerId);
            throw new InvalidOperationException("Broker not found");
        }

        if (broker.Status == BrokerStatus.Inactive)
        {
            _logger.LogWarning("Broker {BrokerId} is already inactive", brokerId);
            throw new InvalidOperationException("Broker is already inactive.");
        }

        broker.Status = BrokerStatus.Inactive;

        await _unitOfWork.Brokers.UpdateAsync(broker);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError("Critical error while deactivating broker {BrokerId}", brokerId);
            throw new InvalidOperationException("Failed to deactivate broker.");
        }

        _logger.LogInformation("Broker {BrokerId} deactivated successfully", brokerId);
    }

    public async Task<GetBrokerByIdDto> GetByIdAsync(Guid brokerId)
    {
        _logger.LogInformation("Retrieving broker {BrokerId}", brokerId);

        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);

        if (broker == null)
        {
            _logger.LogWarning("Broker {BrokerId} not found", brokerId);
            throw new InvalidOperationException("Broker not found");
        }

        _logger.LogInformation("Broker {BrokerId} retrieved successfully", brokerId);

        return _mapper.Map<GetBrokerByIdDto>(broker);
    }

    public async Task<IReadOnlyList<GetBrokersDto>> GetAllAsync()
    {
        var brokers = await _unitOfWork.Brokers.GetAllAsync();
        _logger.LogInformation("Retrieved {Count} brokers", brokers.Count);
        return _mapper.Map<IReadOnlyList<GetBrokersDto>>(brokers);
    }
}
