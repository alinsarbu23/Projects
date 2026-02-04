namespace Insurance.Application.DTOs.RiskFactors;

public class UpdateRiskFactorConfigurationDto
{
    public decimal AdjustmentPercentage { get; set; }

    public bool IsActive { get; set; }
}
