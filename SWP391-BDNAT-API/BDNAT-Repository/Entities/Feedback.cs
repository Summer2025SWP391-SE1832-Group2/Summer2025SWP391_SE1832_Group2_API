using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public int? UserId { get; set; }

    public string? Status { get; set; }

    public string? Content { get; set; }

    public virtual User? User { get; set; }
}
