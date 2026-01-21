namespace Insurance.Application.DTOs.Clients;

public class ClientBuildingSummaryDto
{
    public Guid Id { get; set; }
    public string City { get; set; }
    public string BuildingType { get; set; }
    public decimal InsuredValue { get; set; }
}
