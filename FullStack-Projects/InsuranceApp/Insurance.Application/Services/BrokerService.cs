using AutoMapper;
using Insurance.Application.DTOs.Brokers;
using Insurance.Application.Exceptions;
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
        _logger.LogInformation($"Creating broker with code {dto.BrokerCode}");

        if (await _unitOfWork.Brokers.GetByCodeAsync(dto.BrokerCode) != null)
        {
            _logger.LogWarning($"Broker creation failed. Duplicate BrokerCode {dto.BrokerCode}");
            throw new ArgumentException("Broker code must be unique");
        }

        if (await _unitOfWork.Brokers.GetByEmailAsync(dto.Email) != null)
        {
            _logger.LogWarning($"Broker creation failed. Duplicate Email {dto.Email}");
            throw new ArgumentException("Broker email must be unique");
        }

        var broker = _mapper.Map<Broker>(dto);
        broker.Id = Guid.NewGuid();
        broker.Status = BrokerStatus.Inactive;

        await _unitOfWork.Brokers.AddAsync(broker);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError($"Critical error while saving broker {dto.BrokerCode}");
            throw new InvalidOperationException("Failed to create broker.");
        }

        _logger.LogInformation($"Broker {broker.Id} created successfully");

        return broker.Id;
    }

    public async Task UpdateAsync(Guid brokerId, UpdateBrokerDto dto)
    {
        _logger.LogInformation($"Updating broker {brokerId}");

        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);

        if (broker == null)
        {
            _logger.LogWarning($"Update failed. Broker {brokerId} not found");
            throw new NotFoundException("Broker not found");
        }

        _mapper.Map(dto, broker);

        await _unitOfWork.Brokers.UpdateAsync(broker);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError($"Critical error while updating broker {brokerId}");
            throw new InvalidOperationException("Failed to update broker.");
        }

        _logger.LogInformation($"Broker {brokerId} updated successfully");
    }

    public async Task ActivateAsync(Guid brokerId)
    {
        _logger.LogInformation($"Activating broker {brokerId}");

        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);

        if (broker == null)
        {
            _logger.LogWarning($"Activation failed. Broker {brokerId} not found");
            throw new NotFoundException("Broker not found");
        }

        if (broker.Status == BrokerStatus.Active)
        {
            _logger.LogWarning($"Broker {brokerId} is already active");
            throw new InvalidOperationException("Broker is already active.");
        }

        broker.Status = BrokerStatus.Active;

        await _unitOfWork.Brokers.UpdateAsync(broker);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError($"Critical error while activating broker {brokerId}");
            throw new InvalidOperationException("Failed to activate broker.");
        }

        _logger.LogInformation($"Broker {brokerId} activated successfully");
    }

    public async Task DeactivateAsync(Guid brokerId)
    {
        _logger.LogInformation($"Deactivating broker {brokerId}");

        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);

        if (broker == null)
        {
            _logger.LogWarning($"Deactivation failed. Broker {brokerId} not found");
            throw new NotFoundException("Broker not found");
        }

        if (broker.Status == BrokerStatus.Inactive)
        {
            _logger.LogWarning($"Broker {brokerId} is already inactive");
            throw new InvalidOperationException("Broker is already inactive.");
        }

        broker.Status = BrokerStatus.Inactive;

        await _unitOfWork.Brokers.UpdateAsync(broker);

        var result = await _unitOfWork.SaveChangesAsync();

        if (result == 0)
        {
            _logger.LogError($"Critical error while deactivating broker {brokerId}");
            throw new InvalidOperationException("Failed to deactivate broker.");
        }

        _logger.LogInformation($"Broker {brokerId} deactivated successfully");
    }

    public async Task<GetBrokerByIdDto> GetByIdAsync(Guid brokerId)
    {
        _logger.LogInformation($"Retrieving broker {brokerId}");

        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);

        if (broker == null)
        {
            _logger.LogWarning($"Broker {brokerId} not found");
            throw new NotFoundException("Broker not found");
        }

        _logger.LogInformation($"Broker {brokerId} retrieved successfully");

        return _mapper.Map<GetBrokerByIdDto>(broker);
    }

    public async Task<IReadOnlyList<GetBrokersDto>> GetAllAsync()
    {
        var brokers = await _unitOfWork.Brokers.GetAllAsync();
        _logger.LogInformation($"Retrieved {brokers.Count} brokers");
        return _mapper.Map<IReadOnlyList<GetBrokersDto>>(brokers);
    }
}
