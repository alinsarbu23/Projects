using System;
using System.Collections.Generic;

namespace Insurance.Infrastructure.DAOs;

public partial class IndividualClient
{
    public Guid Id { get; set; }

    public Guid ClientId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Cnp { get; set; } = null!;

    public virtual Client Client { get; set; } = null!;
}
