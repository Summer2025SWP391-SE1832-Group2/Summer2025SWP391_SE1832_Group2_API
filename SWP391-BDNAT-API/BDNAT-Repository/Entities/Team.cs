using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Team
{
    public int TeamId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<DoctorTeam> DoctorTeams { get; set; } = new List<DoctorTeam>();

    public virtual ICollection<TeamService> TeamServices { get; set; } = new List<TeamService>();
}
