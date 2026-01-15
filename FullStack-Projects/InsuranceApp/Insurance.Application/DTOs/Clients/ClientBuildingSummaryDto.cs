namespace Insurance.Application.DTOs.Clients;

public class ClientBuildingSummaryDto
{
    public Guid Id { get; init; }
    public string City { get; init; }
    public string BuildingType { get; init; }
    public decimal InsuredValue { get; init; }
}
