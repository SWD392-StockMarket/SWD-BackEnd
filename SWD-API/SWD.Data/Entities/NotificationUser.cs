using System;
using System.Collections.Generic;

namespace SWD.Data.Entities;

public partial class NotificationUser
{
    public int NotificationId { get; set; }

    public int UserId { get; set; }

    public string? Status { get; set; }

    public virtual Notification Notification { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
