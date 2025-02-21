using System;
using System.Collections.Generic;

namespace SWD.Data.Entities;

public partial class WatchList
{
    public int WatchListId { get; set; }

    public int? UserId { get; set; }

    public string? Label { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? LastEdited { get; set; }

    public string? Status { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
