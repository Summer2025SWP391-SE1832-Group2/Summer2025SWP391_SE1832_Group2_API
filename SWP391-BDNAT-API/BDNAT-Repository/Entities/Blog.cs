using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Blog
{
    public int BlogId { get; set; }

    public int? CreateBy { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public int? BlogTypeId { get; set; }

    public virtual BlogsType? BlogType { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual User? CreateByNavigation { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
}
