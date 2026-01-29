namespace Insurance.Application.DTOs.Geography;

public class GetCountiesByCountryDto
{
    public Guid Id { get; init; }
    public Guid CountryId { get; init; }
    public required string Name { get; init; }
}
