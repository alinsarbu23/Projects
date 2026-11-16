namespace Insurance.Infrastructure.DAOs;

public partial class City
{
    public Guid Id { get; set; }

    public Guid CountyId { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();

    public virtual County County { get; set; } = null!;
}
