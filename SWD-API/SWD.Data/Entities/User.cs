using System;
using System.Collections.Generic;

namespace SWD.Data.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Role { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastEdited { get; set; }

    public string? SubscriptionStatus { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<NotificationUser> NotificationUsers { get; set; } = new List<NotificationUser>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<WatchList> WatchLists { get; set; } = new List<WatchList>();
}
