using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Booking
{
    public int BookingId { get; set; }

    public int? ServiceTypeId { get; set; }

    public int? UserId { get; set; }

    public DateTime? BookingDate { get; set; }

    public string? SampleMethod { get; set; }

    public string? Status { get; set; }

    public string? PaymentStatus { get; set; }

    public DateTime? PreferredDate { get; set; }

    public string? Result { get; set; }

    public virtual ICollection<KitOrder> KitOrders { get; set; } = new List<KitOrder>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();

    public virtual ServiceType? ServiceType { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User? User { get; set; }
}
