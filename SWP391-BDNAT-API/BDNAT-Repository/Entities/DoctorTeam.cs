using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class DoctorTeam
{
    public int DoctorTeamId { get; set; }

    public int? UserId { get; set; }

    public int? TeamId { get; set; }

    public virtual Team? Team { get; set; }

    public virtual User? User { get; set; }
}
