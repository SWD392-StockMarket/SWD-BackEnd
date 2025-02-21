using System;
using System.Collections.Generic;

namespace SWD.Data.Entities;

public partial class Stock
{
    public int StockId { get; set; }

    public int? CompanyId { get; set; }

    public string StockSymbol { get; set; } = null!;

    public int? MarketId { get; set; }

    public DateTime? ListedDate { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Market? Market { get; set; }

    public virtual ICollection<StockInSession> StockInSessions { get; set; } = new List<StockInSession>();

    public virtual ICollection<WatchList> WatchLists { get; set; } = new List<WatchList>();
}
