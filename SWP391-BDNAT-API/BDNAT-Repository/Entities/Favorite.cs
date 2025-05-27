using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class Favorite
{
    public int FavoriteId { get; set; }

    public int? UserId { get; set; }

    public int? BlogId { get; set; }

    public virtual Blog? Blog { get; set; }

    public virtual User? User { get; set; }
}
