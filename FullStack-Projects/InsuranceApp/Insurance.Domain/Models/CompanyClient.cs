namespace Insurance.Domain.Models;

using Insurance.Domain.Enums;

public class CompanyClient
{
    public Guid Id { get; set; }

    public ClientType ClientType { get; set; } = ClientType.Company;

    public string CompanyName { get; set; } = null!;

    public string Cui { get; set; } = null!;

    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Address { get; set; } = null!;
}
