
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD.Data.Entities;

public partial class NotificationUser
{
    [Key]
    [Column(Order = 1)]
    [ForeignKey("Notification")]
    public int NotificationId { get; set; }
    [Key]
    [Column(Order = 2)]
    [ForeignKey("User")]
    public int UserId { get; set; }

    public string? Status { get; set; }

    public virtual Notification Notification { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
