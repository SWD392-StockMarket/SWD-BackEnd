using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD.Data.Entities;

public partial class Notification
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NotificationId { get; set; }
    [ForeignKey("User")]
    public int? StaffId { get; set; }

    public string? Content { get; set; }

    public string? Navigation { get; set; }

    public string? Type { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual ICollection<NotificationUser> NotificationUsers { get; set; } = new List<NotificationUser>();

    public virtual User? Staff { get; set; }
}
