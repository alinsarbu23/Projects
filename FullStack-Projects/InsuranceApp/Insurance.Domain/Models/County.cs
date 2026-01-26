namespace Insurance.Domain.Models.Geography;

public class County
{
    public Guid Id { get; set; }
    public Guid CountryId { get; set; }
    public string Name { get; set; } = null!;
}
