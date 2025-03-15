using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD.Data.Entities;

public partial class DeviceToken
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [ForeignKey("User")]
    public int? UserId { get; set; }
    
    public string? FCMToken { get; set; }
    
    public virtual User? User { get; set; }
}