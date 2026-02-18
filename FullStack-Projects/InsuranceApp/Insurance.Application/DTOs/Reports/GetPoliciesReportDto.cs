using Insurance.Domain.Enums;

namespace Insurance.Application.DTOs.Reports;

public class GetPoliciesReportDto
{
    public DateTime? From { get; init; }
    public DateTime? To { get; init; }
    public PolicyStatus? Status { get; init; }
    public string? CurrencyCode { get; init; }
    public Guid? BuildingTypeId { get; init; }
}
