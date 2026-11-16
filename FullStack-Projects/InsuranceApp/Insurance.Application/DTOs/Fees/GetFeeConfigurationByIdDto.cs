namespace Insurance.Application.DTOs.Fees;

public class GetFeeConfigurationByIdDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;

    public decimal Percentage { get; set; }

    public string AppliesToRisk { get; set; } = string.Empty;

    public bool IsActive { get; set; }
}
