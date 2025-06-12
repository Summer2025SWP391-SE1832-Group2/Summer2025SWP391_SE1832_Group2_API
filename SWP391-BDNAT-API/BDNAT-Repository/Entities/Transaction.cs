using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? BookingId { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public int? UserId { get; set; }

    public long OrderCode { get; set; }

    public string? TransactionCode { get; set; }

    public string PaymentGateway { get; set; } = null!;

    public string Status { get; set; } = null!;

    public string? PaymentMethod { get; set; }

    public string? PaymentUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual User? User { get; set; }
}
