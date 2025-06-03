using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class UserWorkSchedule
{
    public int UserWorkScheduleId { get; set; }

    public int? UserId { get; set; }

    public int? WorkScheduleId { get; set; }

    public DateTime? Date { get; set; }

    public virtual User? User { get; set; }

    public virtual WorkSchedule? WorkSchedule { get; set; }
}
