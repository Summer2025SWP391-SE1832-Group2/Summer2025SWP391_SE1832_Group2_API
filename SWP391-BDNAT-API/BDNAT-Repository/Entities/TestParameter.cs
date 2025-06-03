using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class TestParameter
{
    public int TestParameterId { get; set; }

    public int? ServiceId { get; set; }

    public int? ParameterId { get; set; }

    public int? DisplayOrder { get; set; }

    public virtual Parameter? Parameter { get; set; }

    public virtual ICollection<ResultDetail> ResultDetails { get; set; } = new List<ResultDetail>();

    public virtual Service? Service { get; set; }
}
