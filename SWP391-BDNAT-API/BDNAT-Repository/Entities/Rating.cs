using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Rating
{
    public int RatingId { get; set; }

    public string? Content { get; set; }

    public int? Vote { get; set; }

    public int? BookingId { get; set; }

    public int CreateBy { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual User CreateByNavigation { get; set; } = null!;
}
