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

    public async Task<IReadOnlyList<GetPoliciesByBrokerReportDto>> GetPoliciesByBrokerAsync(GetPoliciesReportDto dto)
    {
        _logger.LogInformation($"Generating broker report (From={dto.From}, To={dto.To}, Status={dto.Status}, Currency={dto.CurrencyCode})");

        var result = await _unitOfWork.Reports.GetPoliciesByBrokerAsync(dto);

        _logger.LogInformation($"Retrieved {result.Count} broker report rows");

        return result;
    }

    public async Task<IReadOnlyList<GetPoliciesByCountryReportDto>> GetPoliciesByCountryAsync(GetPoliciesReportDto dto)
    {
        _logger.LogInformation($"Generating country report");

        var result = await _unitOfWork.Reports.GetPoliciesByCountryAsync(dto);

        _logger.LogInformation($"Retrieved {result.Count} country report rows");

        return result;
    }

    public async Task<IReadOnlyList<GetPoliciesByCountyReportDto>> GetPoliciesByCountyAsync(GetPoliciesReportDto dto)
    {
        _logger.LogInformation($"Generating county report");

        var result = await _unitOfWork.Reports.GetPoliciesByCountyAsync(dto);

        _logger.LogInformation($"Retrieved {result.Count} county report rows");

        return result;
    }

    public async Task<IReadOnlyList<GetPoliciesByCityReportDto>> GetPoliciesByCityAsync(GetPoliciesReportDto dto)
    {
        _logger.LogInformation($"Generating city report");

        var result = await _unitOfWork.Reports.GetPoliciesByCityAsync(dto);

        _logger.LogInformation($"Retrieved {result.Count} city report rows");

        return result;
    }
}
