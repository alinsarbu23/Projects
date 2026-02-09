namespace Insurance.Application.DTOs.Policies;

public class GetPoliciesDto
{
    public Guid Id { get; set; }

    public string PolicyNumber { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public decimal FinalPremium { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }
}
