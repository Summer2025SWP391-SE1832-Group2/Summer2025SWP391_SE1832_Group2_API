using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? BookingId { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? DurationDays { get; set; }

    public virtual Booking? Booking { get; set; }
}
