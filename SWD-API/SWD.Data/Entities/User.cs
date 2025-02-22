using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SWD.Data.Entities;

public partial class User  : IdentityUser<int>
{ 
    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastEdited { get; set; }

    public string? SubscriptionStatus { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<NotificationUser> NotificationUsers { get; set; } = new List<NotificationUser>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<WatchList> WatchLists { get; set; } = new List<WatchList>();
}
