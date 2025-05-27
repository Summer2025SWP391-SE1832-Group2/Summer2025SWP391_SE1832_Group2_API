using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Comment
{
    public int UniqueId { get; set; }

    public int UserId { get; set; }

    public int BlogId { get; set; }

    public string Comment1 { get; set; } = null!;

    public int? RootId { get; set; }

    public virtual Blog Blog { get; set; } = null!;

    public virtual ICollection<Comment> InverseRoot { get; set; } = new List<Comment>();

    public virtual Comment? Root { get; set; }

    public virtual User User { get; set; } = null!;
}
