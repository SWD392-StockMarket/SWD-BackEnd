using System;
using System.Collections.Generic;

namespace SWD.Data.Entities;

public partial class Session
{
    public int SessionId { get; set; }

    public string? SessionType { get; set; }

    public DateTime? StartTime { get; set; }

    public DateTime? EndTime { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<StockInSession> StockInSessions { get; set; } = new List<StockInSession>();
}
