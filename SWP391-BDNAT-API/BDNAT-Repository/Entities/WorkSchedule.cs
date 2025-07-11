﻿using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class WorkSchedule
{
    public int WorkScheduleId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public TimeSpan? StartTime { get; set; }

    public TimeSpan? EndTime { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public virtual ICollection<UserWorkSchedule> UserWorkSchedules { get; set; } = new List<UserWorkSchedule>();
}
