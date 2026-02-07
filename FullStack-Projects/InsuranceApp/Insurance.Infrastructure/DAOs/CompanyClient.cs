namespace Insurance.Infrastructure.DAOs;

public partial class CompanyClient
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string Cui { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;
}
