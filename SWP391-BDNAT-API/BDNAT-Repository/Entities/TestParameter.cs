using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class TestParameter
{
    public int TestParameterId { get; set; }

    public int? ServiceTypeId { get; set; }

    public int? ParameterId { get; set; }

    public int? DisplayOrder { get; set; }

    public virtual Parameter? Parameter { get; set; }

    public virtual ICollection<Result> Results { get; set; } = new List<Result>();

    public virtual ServiceType? ServiceType { get; set; }
}
