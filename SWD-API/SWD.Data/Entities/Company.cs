using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SWD.Data.Entities;

public class Company
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CompanyId { get; set; }
    [Required]
    public string CompanyName { get; set; } = null!;

    public string? Ceo { get; set; }

    public string? Information { get; set; }
    [JsonIgnore]
    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
