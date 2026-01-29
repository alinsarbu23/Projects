namespace Insurance.Application.DTOs.Clients;

public class ClientBuildingSummaryDto
{
    public Guid Id { get; set; }
    public required string City { get; init; }
    public required string BuildingType { get; init; }
    public decimal InsuredValue { get; set; }
}
