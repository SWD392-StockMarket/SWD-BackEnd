using System;
using System.Collections.Generic;

namespace SWD.Data.Entities;

public partial class Market
{
    public int MarketId { get; set; }

    public string MarketName { get; set; } = null!;

    public string? Address { get; set; }

    public DateTime? EstablishedDate { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Website { get; set; }

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
