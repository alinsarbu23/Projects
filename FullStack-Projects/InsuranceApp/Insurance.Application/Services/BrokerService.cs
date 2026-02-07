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
        if (await _unitOfWork.Brokers.GetByCodeAsync(dto.BrokerCode) != null)
        {
            throw new ArgumentException("Broker code must be unique");
        }

        if (await _unitOfWork.Brokers.GetByEmailAsync(dto.Email) != null)
        {
            throw new ArgumentException("Broker email must be unique");
        }

        var broker = _mapper.Map<Broker>(dto);
        broker.Id = Guid.NewGuid();
        broker.Status = BrokerStatus.Inactive;

        await _unitOfWork.Brokers.AddAsync(broker);
        await _unitOfWork.SaveChangesAsync();

        return broker.Id;
    }

    public async Task UpdateAsync(Guid brokerId, UpdateBrokerDto dto)
    {
        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);
        if (broker == null)
        {
            throw new InvalidOperationException("Broker not found");
        }

        _mapper.Map(dto, broker);

        await _unitOfWork.Brokers.UpdateAsync(broker);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Broker {BrokerId} updated", brokerId);
    }

    public async Task ActivateAsync(Guid brokerId)
    {
        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);
        if (broker == null)
        {
            throw new InvalidOperationException("Broker not found");
        }

        broker.Status = BrokerStatus.Active;

        await _unitOfWork.Brokers.UpdateAsync(broker);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Broker {BrokerId} activated", brokerId);
    }

    public async Task DeactivateAsync(Guid brokerId)
    {
        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);
        if (broker == null)
        {
            throw new InvalidOperationException("Broker not found");
        }

        broker.Status = BrokerStatus.Inactive;

        await _unitOfWork.Brokers.UpdateAsync(broker);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Broker {BrokerId} deactivated", brokerId);
    }

    public async Task<GetBrokerByIdDto> GetByIdAsync(Guid brokerId)
    {
        var broker = await _unitOfWork.Brokers.GetByIdAsync(brokerId);
        if (broker == null)
        {
            throw new InvalidOperationException("Broker not found");
        }

        _logger.LogInformation("Broker {BrokerId} retrieved", brokerId);
        return _mapper.Map<GetBrokerByIdDto>(broker);
    }

    public async Task<IReadOnlyList<GetBrokersDto>> GetAllAsync()
    {
        var brokers = await _unitOfWork.Brokers.GetAllAsync();
        _logger.LogInformation("Retrieved {Count} brokers", brokers.Count);
        return _mapper.Map<IReadOnlyList<GetBrokersDto>>(brokers);
    }
}
