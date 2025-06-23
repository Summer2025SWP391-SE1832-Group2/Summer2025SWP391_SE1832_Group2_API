using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Booking
{
    public int BookingId { get; set; }

    public int ServiceId { get; set; }

    public int UserId { get; set; }

    public DateTime BookingDate { get; set; }

    public string Status { get; set; } = null!;

    public string PaymentStatus { get; set; } = null!;

    public DateTime PreferredDate { get; set; }

    public bool BuyKit { get; set; }

    public string Method { get; set; } = null!;

    public string? DocumentsVerify { get; set; }

    public long? OrderCode { get; set; }

    public string? FinalResult { get; set; }

    public virtual ICollection<KitOrder> KitOrders { get; set; } = new List<KitOrder>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<ResultDetail> ResultDetails { get; set; } = new List<ResultDetail>();

    public virtual ICollection<SampleCollectionSchedule> SampleCollectionSchedules { get; set; } = new List<SampleCollectionSchedule>();

    public virtual ICollection<SampleShipment> SampleShipments { get; set; } = new List<SampleShipment>();

    public virtual ICollection<Sample> Samples { get; set; } = new List<Sample>();

    public virtual Service Service { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User User { get; set; } = null!;
}
