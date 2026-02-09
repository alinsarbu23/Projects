using Insurance.Domain.Enums;

namespace Insurance.Domain.Models;

public class Administrator
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Email { get; set; }

    public AdminRole Role { get; set; }
}
