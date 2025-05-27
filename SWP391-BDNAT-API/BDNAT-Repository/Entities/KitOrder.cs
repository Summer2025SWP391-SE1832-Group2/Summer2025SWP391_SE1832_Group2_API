using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class KitOrder
{
    public int KitOrderId { get; set; }

    public int? TestKitId { get; set; }

    public int? BookingId { get; set; }

    public int? ShippingId { get; set; }

    public string? Status { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual ShippingOrder? Shipping { get; set; }

    public virtual TestKit? TestKit { get; set; }
}
