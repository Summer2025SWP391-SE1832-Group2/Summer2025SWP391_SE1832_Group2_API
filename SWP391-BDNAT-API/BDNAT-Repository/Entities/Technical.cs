using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Technical
{
    public int TechnicalId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public string? Detail { get; set; }

    public string? Img { get; set; }

    public virtual ICollection<TechnicalService> TechnicalServices { get; set; } = new List<TechnicalService>();
}
