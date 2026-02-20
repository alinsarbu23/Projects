using Insurance.Application.DTOs.Reports;

namespace Insurance.Application.Interfaces.Repositories;

public interface IReportsRepository
{
    Task<IReadOnlyList<GetPoliciesByBrokerReportDto>> GetPoliciesByBrokerAsync(GetPoliciesReportDto filter);

    Task<IReadOnlyList<GetPoliciesByCountryReportDto>> GetPoliciesByCountryAsync(GetPoliciesReportDto filter);

    Task<IReadOnlyList<GetPoliciesByCountyReportDto>> GetPoliciesByCountyAsync(GetPoliciesReportDto filter);

    Task<IReadOnlyList<GetPoliciesByCityReportDto>> GetPoliciesByCityAsync(GetPoliciesReportDto filter);
}
