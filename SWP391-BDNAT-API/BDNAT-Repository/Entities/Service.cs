using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Service
{
    public int ServiceId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? DurationDays { get; set; }

    public decimal? Price { get; set; }

    public int? ServiceTypeId { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ServiceType? ServiceType { get; set; }

    public virtual ICollection<TeamService> TeamServices { get; set; } = new List<TeamService>();

    public virtual ICollection<TechnicalService> TechnicalServices { get; set; } = new List<TechnicalService>();
}
