
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD.Data.Entities;

public partial class StockHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StockHistoryId { get; set; }

    public string StockSymbol { get; set; } = null!;

    public long? OutstandingShares { get; set; }

    public string? Osreasonchange { get; set; }

    public long? ListedShares { get; set; }

    public string? Lsreasonchange { get; set; }

    public decimal? RegisteredCapital { get; set; }

    public string? Rcreasonchange { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.Now;
}
