using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class TestKit
{
    public int TestKitId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? IncludeItems { get; set; }

    public bool? IsActive { get; set; }

    public virtual ICollection<KitOrder> KitOrders { get; set; } = new List<KitOrder>();
}
