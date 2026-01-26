namespace Insurance.Domain.Models;

using Insurance.Domain.Enums;

public class Client
{
    public Guid Id { get; set; }
    public ClientType ClientType { get; set; }
    public string Name { get; set; } = null!;
    public string? Cnp { get; set; }
    public string? Cui { get; set; }
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Address { get; set; } = null!;
}
