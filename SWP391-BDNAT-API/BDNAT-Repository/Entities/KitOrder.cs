using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class KitOrder
{
    public int KitOrderId { get; set; }

    public int TestKitId { get; set; }

    public int BookingId { get; set; }

    public string Status { get; set; } = null!;

    public int Quantity { get; set; }

    public string ShipToAdress { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public DateTime OrderDate { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual TestKit TestKit { get; set; } = null!;
}
