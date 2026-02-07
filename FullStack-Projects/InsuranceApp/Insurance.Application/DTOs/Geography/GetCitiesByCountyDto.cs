namespace Insurance.Application.DTOs.Geography;

public class GetCitiesByCountyDto
{
    public Guid Id { get; init; }

    public Guid CountyId { get; init; }

    public required string Name { get; init; }

    public required string Code { get; init; }
}
