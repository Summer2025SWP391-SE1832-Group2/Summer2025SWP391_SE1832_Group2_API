using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class ShippingOrder
{
    public int ShippingId { get; set; }

    public string? Receiver { get; set; }

    public string? Address { get; set; }

    public string? ShipperName { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<KitOrder> KitOrders { get; set; } = new List<KitOrder>();
}
