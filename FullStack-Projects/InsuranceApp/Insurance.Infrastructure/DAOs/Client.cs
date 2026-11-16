namespace Insurance.Infrastructure.DAOs;

public class Client
{
    public Guid Id { get; set; }

    public int ClientTypeId { get; set; }

    public ClientType ClientType { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Cnp { get; set; }

    public string? Cui { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public ICollection<Building> Buildings { get; set; } = new List<Building>();
}
