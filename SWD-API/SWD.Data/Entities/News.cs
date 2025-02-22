using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD.Data.Entities;

public partial class News
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NewsId { get; set; }
    [ForeignKey("User")]
    public int? StaffId { get; set; }

    public string? Content { get; set; }

    public string? Type { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastEdited { get; set; }

    public string? Status { get; set; }

    public string? Url { get; set; }

    public virtual User? Staff { get; set; }
}
