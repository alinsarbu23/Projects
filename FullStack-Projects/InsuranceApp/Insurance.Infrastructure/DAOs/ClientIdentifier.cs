namespace Insurance.Infrastructure.DAOs;

public class ClientIdentifier
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public string OldIdentifier { get; set; } = null!;
    public string NewIdentifier { get; set; } = null!;
    public string IdentifierType { get; set; } = null!;
    public Guid? ChangedByUserId { get; set; }
    public DateTime ChangedAt { get; set; }
}
