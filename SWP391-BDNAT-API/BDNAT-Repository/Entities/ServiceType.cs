using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class ServiceType
{
    public int ServiceTypeId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();
}
