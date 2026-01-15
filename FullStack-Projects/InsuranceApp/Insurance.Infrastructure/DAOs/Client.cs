using System;
using System.Collections.Generic;

namespace Insurance.Infrastructure.DAOs;

public partial class Client
{
    public Guid Id { get; set; }

    public string ClientType { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();

    public virtual ICollection<CompanyClient> CompanyClients { get; set; } = new List<CompanyClient>();

    public virtual ICollection<IndividualClient> IndividualClients { get; set; } = new List<IndividualClient>();
}
