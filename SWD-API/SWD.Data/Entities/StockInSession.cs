using System;
using System.Collections.Generic;

namespace SWD.Data.Entities;

public partial class StockInSession
{
    public int StockInSessionId { get; set; }

    public int? StockId { get; set; }

    public int? SessionId { get; set; }

    public DateTime? DateTime { get; set; }

    public decimal? OpenPrice { get; set; }

    public decimal? ClosePrice { get; set; }

    public decimal? HighPrice { get; set; }

    public decimal? LowPrice { get; set; }

    public virtual Session? Session { get; set; }

    public virtual Stock? Stock { get; set; }
}
