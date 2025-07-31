using System;
using System.Collections.Generic;

namespace BDNAT_Repository.Entities;

public partial class UserNotificationToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Token { get; set; } = null!;

    public DateTime? LastUpdated { get; set; }

    public bool? IsRevoked { get; set; }

    public virtual User User { get; set; } = null!;
}
