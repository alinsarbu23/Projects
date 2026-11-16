using Insurance.Application.DTOs.Reports;

namespace Insurance.Application.Interfaces.Repositories;

public interface IReportsRepository
{
    Task<(IReadOnlyList<GetPoliciesByCountryReportDto> Items, int TotalCount)> GetPoliciesByCountryAsync(GetPoliciesReportDto filter, int page, int pageSize);

    Task<(IReadOnlyList<GetPoliciesByCountyReportDto> Items, int TotalCount)> GetPoliciesByCountyAsync(GetPoliciesReportDto filter, int page, int pageSize);

    Task<(IReadOnlyList<GetPoliciesByCityReportDto> Items, int TotalCount)> GetPoliciesByCityAsync(GetPoliciesReportDto filter, int page, int pageSize);

    Task<(IReadOnlyList<GetPoliciesByBrokerReportDto> Items, int TotalCount)> GetPoliciesByBrokerAsync(GetPoliciesReportDto filter, int page, int pageSize);
}
