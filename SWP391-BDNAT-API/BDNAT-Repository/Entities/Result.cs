using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Result
{
    public int ResultId { get; set; }

    public int? BookingId { get; set; }

    public int? TestParameterId { get; set; }

    public string? Value { get; set; }

    public string? Note { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual TestParameter? TestParameter { get; set; }
}
