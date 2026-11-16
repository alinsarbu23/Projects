using Insurance.Application.DTOs.Reports;

namespace Insurance.Application.Interfaces.Services;

public interface IReportsService
{
    Task<PagedResultDto<GetPoliciesByBrokerReportDto>> GetPoliciesByBrokerAsync(GetPoliciesReportDto dto, int page, int pageSize);

    Task<PagedResultDto<GetPoliciesByCountryReportDto>> GetPoliciesByCountryAsync(GetPoliciesReportDto dto, int page, int pageSize);

    Task<PagedResultDto<GetPoliciesByCountyReportDto>> GetPoliciesByCountyAsync(GetPoliciesReportDto dto, int page, int pageSize);

    Task<PagedResultDto<GetPoliciesByCityReportDto>> GetPoliciesByCityAsync(GetPoliciesReportDto dto, int page, int pageSize);
}
