using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class ResultDetail
{
    public int ResultDetailId { get; set; }

    public int BookingId { get; set; }

    public int TestParameterId { get; set; }

    public string? Value { get; set; }

    public int SampleId { get; set; }

    public virtual Booking Booking { get; set; } = null!;

    public virtual Sample Sample { get; set; } = null!;

    public virtual TestParameter TestParameter { get; set; } = null!;
}
