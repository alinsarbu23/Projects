namespace Insurance.Application.DTOs.Geography;

public class GetCitiesByCountyDto
{
    public Guid Id { get; init; }
    public Guid CountyId { get; init; }
    public string Name { get; init; }
    public string Code { get; init; }
}
