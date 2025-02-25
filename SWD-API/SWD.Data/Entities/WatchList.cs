using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SWD.Data.Entities;

public partial class WatchList
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int WatchListId { get; set; }
    [ForeignKey("User")]
    public int? UserId { get; set; }

    public string? Label { get; set; }

    public DateTime? CreatedDate { get; set; } = DateTime.Now;

    public DateTime? LastEdited { get; set; } = DateTime.Now;

    public string? Status { get; set; }

    public virtual User? User { get; set; }
    [JsonIgnore]
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
