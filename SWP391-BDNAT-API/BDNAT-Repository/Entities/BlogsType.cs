using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class BlogsType
{
    public int BlogTypeId { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();
}
