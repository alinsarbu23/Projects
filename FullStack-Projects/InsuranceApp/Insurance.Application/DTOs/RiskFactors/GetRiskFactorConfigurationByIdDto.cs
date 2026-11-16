namespace Insurance.Application.DTOs.RiskFactors;

public class GetRiskFactorConfigurationByIdDto
{
    public Guid Id { get; set; }

    public string Level { get; set; } = string.Empty;

    public decimal AdjustmentPercentage { get; set; }

    public bool IsActive { get; set; }
}
