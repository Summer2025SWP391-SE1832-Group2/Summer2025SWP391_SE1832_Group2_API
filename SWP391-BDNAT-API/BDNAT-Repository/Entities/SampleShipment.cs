using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class SampleShipment
{
    public int SampleShipmentId { get; set; }

    public int BookingId { get; set; }

    public string Address { get; set; } = null!;

    public DateTime ShipDate { get; set; }

    public DateTime Status { get; set; }

    public int? CollectedBy { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual User? CollectedByNavigation { get; set; }
}
