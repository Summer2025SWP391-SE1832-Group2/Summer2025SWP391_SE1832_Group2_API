using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class TechnicalService
{
    public int TechServiceId { get; set; }

    public int TechnicalId { get; set; }

    public int ServiceId { get; set; }

    public virtual Service Service { get; set; } = null!;

    public virtual Technical Technical { get; set; } = null!;
}
