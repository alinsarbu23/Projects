namespace Insurance.Domain.Models;

using Insurance.Domain.Enums;

public class IndividualClient
{
    public Guid Id { get; set; }

    public ClientType ClientType { get; set; } = ClientType.Individual;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;

    public string Cnp { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Address { get; set; } = null!;
}
