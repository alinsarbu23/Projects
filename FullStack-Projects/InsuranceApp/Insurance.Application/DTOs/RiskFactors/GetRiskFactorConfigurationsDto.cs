namespace Insurance.Application.DTOs.RiskFactors;

public class GetRiskFactorConfigurationsDto
{
    public Guid Id { get; set; }

    public string Level { get; set; } = string.Empty;
}
