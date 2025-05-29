using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class SampleCollectionSchedule
{
    public int ScheduleId { get; set; }

    public int BookingId { get; set; }

    public int? CollectorId { get; set; }

    public DateTime CollectionDate { get; set; }

    public string Location { get; set; } = null!;

    public bool? CollectedByUser { get; set; }

    public string? Status { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual User? Collector { get; set; }
}
