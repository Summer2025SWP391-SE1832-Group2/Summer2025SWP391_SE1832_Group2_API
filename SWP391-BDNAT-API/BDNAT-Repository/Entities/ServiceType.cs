using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class ServiceType
{
    public int ServiceTypeId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? ServiceId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Service? Service { get; set; }

    public virtual ICollection<TestParameter> TestParameters { get; set; } = new List<TestParameter>();
}
