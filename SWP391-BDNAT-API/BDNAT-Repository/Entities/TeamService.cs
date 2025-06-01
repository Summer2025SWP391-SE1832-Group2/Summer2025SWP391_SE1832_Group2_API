using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class TeamService
{
    public int TeamServiceId { get; set; }

    public int? TeamId { get; set; }

    public int? ServiceId { get; set; }

    public virtual Service? Service { get; set; }

    public virtual Team? Team { get; set; }
}
