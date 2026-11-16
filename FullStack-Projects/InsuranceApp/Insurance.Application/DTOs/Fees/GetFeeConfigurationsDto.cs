namespace Insurance.Application.DTOs.Fees;

public class GetFeeConfigurationsDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Type { get; set; } = string.Empty;
}
