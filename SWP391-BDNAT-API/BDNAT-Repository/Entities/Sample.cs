using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Sample
{
    public int SampleId { get; set; }

    public int? BookingId { get; set; }

    public int? CollectedBy { get; set; }

    public DateTime? CollectedDate { get; set; }

    public string? SampleType { get; set; }

    public string? ParticipantName { get; set; }

    public string? Notes { get; set; }

    public string? Picture { get; set; }

    public string? Transport { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual User? CollectedByNavigation { get; set; }

    public virtual ICollection<ResultDetail> ResultDetails { get; set; } = new List<ResultDetail>();
}
