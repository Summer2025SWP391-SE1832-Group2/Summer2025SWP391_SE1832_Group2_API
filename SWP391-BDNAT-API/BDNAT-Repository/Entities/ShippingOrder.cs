using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class ShippingOrder
{
    public int ShippingId { get; set; }

    public string? Receiver { get; set; }

    public string? Address { get; set; }

    public int? ShipperId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public int? BookingId { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual User? Shipper { get; set; }
}
