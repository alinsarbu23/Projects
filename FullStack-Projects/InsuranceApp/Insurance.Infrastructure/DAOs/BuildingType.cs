using System;
using System.Collections.Generic;

namespace Insurance.Infrastructure.DAOs;

public partial class BuildingType
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();
}
