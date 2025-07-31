using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Notification
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Title { get; set; }

    public string? Body { get; set; }

    public DateTime? ReceivedAt { get; set; }

    public bool? IsRead { get; set; }

    public virtual User User { get; set; } = null!;
}
