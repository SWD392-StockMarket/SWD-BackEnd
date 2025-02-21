using System;
using System.Collections.Generic;

namespace SWD.Data.Entities;

public partial class News
{
    public int NewsId { get; set; }

    public int? StaffId { get; set; }

    public string? Content { get; set; }

    public string? Type { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastEdited { get; set; }

    public string? Status { get; set; }

    public string? Url { get; set; }

    public virtual User? Staff { get; set; }
}
