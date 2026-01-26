namespace Insurance.Domain.Models.Geography;

public class City
{
    public Guid Id { get; set; }
    public Guid CountyId { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
}
