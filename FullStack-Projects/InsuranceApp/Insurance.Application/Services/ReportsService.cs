using Insurance.Application.DTOs.Reports;
using Insurance.Application.Interfaces.Repositories;
using Insurance.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace Insurance.Application.Services;

public class ReportsService : IReportsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReportsService> _logger;

    public ReportsService(IUnitOfWork unitOfWork, ILogger<ReportsService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    private void Validate(GetPoliciesReportDto dto)
    {
        if (dto.From.HasValue && dto.To.HasValue)
        {
            if (dto.From > dto.To)
            {
                throw new ArgumentException("From date cannot be greater than To date.");
            }
        }
    }

    public async Task<PagedResultDto<GetPoliciesByBrokerReportDto>> GetPoliciesByBrokerAsync(GetPoliciesReportDto dto, int page, int pageSize)
    {
        Validate(dto);

        _logger.LogInformation($"Generating broker report (From={dto.From}, To={dto.To}, Status={dto.Status}, Currency={dto.CurrencyCode})");

        var result = await _unitOfWork.Reports.GetPoliciesByBrokerAsync(dto);

        var totalCount = result.Count;

        if (totalCount == 0)
        {
            _logger.LogWarning("Broker report returned no data");
        }
        else
        {
            _logger.LogInformation($"Broker report returned {totalCount} rows");
        }

        var items = result
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResultDto<GetPoliciesByBrokerReportDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }

    public async Task<PagedResultDto<GetPoliciesByCountryReportDto>> GetPoliciesByCountryAsync(GetPoliciesReportDto dto, int page, int pageSize)
    {
        Validate(dto);

        _logger.LogInformation($"Generating country report (From={dto.From}, To={dto.To})");

        var result = await _unitOfWork.Reports.GetPoliciesByCountryAsync(dto);

        var totalCount = result.Count;

        if (totalCount == 0)
        {
            _logger.LogWarning("Country report returned no data");
        }
        else
        {
            _logger.LogInformation($"Country report returned {totalCount} rows");
        }

        var items = result
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResultDto<GetPoliciesByCountryReportDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }

    public async Task<PagedResultDto<GetPoliciesByCountyReportDto>> GetPoliciesByCountyAsync(GetPoliciesReportDto dto, int page, int pageSize)
    {
        Validate(dto);

        _logger.LogInformation($"Generating county report (From={dto.From}, To={dto.To})");

        var result = await _unitOfWork.Reports.GetPoliciesByCountyAsync(dto);

        var totalCount = result.Count;

        if (totalCount == 0)
        {
            _logger.LogWarning("County report returned no data");
        }
        else
        {
            _logger.LogInformation($"County report returned {totalCount} rows");
        }

        var items = result
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResultDto<GetPoliciesByCountyReportDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }

    public async Task<PagedResultDto<GetPoliciesByCityReportDto>> GetPoliciesByCityAsync(GetPoliciesReportDto dto, int page, int pageSize)
    {
        Validate(dto);

        _logger.LogInformation($"Generating city report (From={dto.From}, To={dto.To})");

        var result = await _unitOfWork.Reports.GetPoliciesByCityAsync(dto);

        var totalCount = result.Count;

        if (totalCount == 0)
        {
            _logger.LogWarning("City report returned no data");
        }
        else
        {
            _logger.LogInformation($"City report returned {totalCount} rows");
        }

        var items = result
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagedResultDto<GetPoliciesByCityReportDto>
        {
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            Items = items
        };
    }
}
