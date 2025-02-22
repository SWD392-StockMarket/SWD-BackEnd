using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SWD.Data.Entities;

public partial class Market
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MarketId { get; set; }
    [Required]
    public string MarketName { get; set; } = null!;

    public string? Address { get; set; }

    public DateTime? EstablishedDate { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Website { get; set; }

    public virtual ICollection<Stock> Stocks { get; set; } = new List<Stock>();
}
