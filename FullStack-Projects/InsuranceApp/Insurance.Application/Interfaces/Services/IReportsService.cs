using Insurance.Application.DTOs.Reports;

namespace Insurance.Application.Interfaces.Services;

public interface IReportsService
{
    Task<IReadOnlyList<GetPoliciesByBrokerReportDto>> GetPoliciesByBrokerAsync(GetPoliciesReportDto dto);

    Task<IReadOnlyList<GetPoliciesByCountryReportDto>> GetPoliciesByCountryAsync(GetPoliciesReportDto dto);

    Task<IReadOnlyList<GetPoliciesByCountyReportDto>> GetPoliciesByCountyAsync(GetPoliciesReportDto dto);

    Task<IReadOnlyList<GetPoliciesByCityReportDto>> GetPoliciesByCityAsync(GetPoliciesReportDto dto);
}
