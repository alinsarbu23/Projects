namespace Insurance.Application.DTOs.Policies;

public class GetPolicyByIdDto
{
    public Guid Id { get; set; }

    public string PolicyNumber { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public decimal BasePremium { get; set; }

    public decimal FinalPremium { get; set; }

    public string CurrencyCode { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string BuildingAddress { get; set; } = string.Empty;
}
