namespace Insurance.Infrastructure.DAOs;

public partial class Country
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<County> Counties { get; set; } = new List<County>();
}
