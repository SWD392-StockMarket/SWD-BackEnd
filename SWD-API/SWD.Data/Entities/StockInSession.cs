
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SWD.Data.Entities;

public partial class StockInSession
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StockInSessionId { get; set; }

    public int? StockId { get; set; }

    public int? SessionId { get; set; }

    public DateTime? DateTime { get; set; }

    public decimal? OpenPrice { get; set; }

    public decimal? ClosePrice { get; set; }

    public decimal? HighPrice { get; set; }

    public decimal? LowPrice { get; set; }
    
    public decimal? Volume { get; set; }
    [JsonIgnore]
    public virtual Session? Session { get; set; }
    [JsonIgnore]
    public virtual Stock? Stock { get; set; }
}
