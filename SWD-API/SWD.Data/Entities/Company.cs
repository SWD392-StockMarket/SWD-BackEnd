using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
