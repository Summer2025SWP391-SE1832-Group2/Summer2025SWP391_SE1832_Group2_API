using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Parameter
{
    public int ParameterId { get; set; }

    public string? Name { get; set; }

    public string? Unit { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TestParameter> TestParameters { get; set; } = new List<TestParameter>();
}
