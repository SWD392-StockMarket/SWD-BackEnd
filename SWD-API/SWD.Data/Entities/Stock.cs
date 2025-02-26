
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SWD.Data.Entities;

public partial class Stock
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StockId { get; set; }

    public int? CompanyId { get; set; }
    [Required]
    public string StockSymbol { get; set; } = null!;

    public int? MarketId { get; set; }

    public DateTime? ListedDate { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Market? Market { get; set; }
    [JsonIgnore]
    public virtual ICollection<StockInSession> StockInSessions { get; set; } = new List<StockInSession>();
    [JsonIgnore]
    public virtual ICollection<WatchList> WatchLists { get; set; } = new List<WatchList>();
}
