using System;
using System.Collections.Generic;

namespace SWD.Data.Entities;

public partial class Company
{
    public int CompanyId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string? Ceo { get; set; }

    public string? Information { get; set; }

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
