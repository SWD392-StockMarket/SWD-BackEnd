using SWD.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.DTOs.Stock
{
    public class StockDTO
    {
        public int StockId { get; set; }

        public int? CompanyId { get; set; }
        public string StockSymbol { get; set; } = string.Empty;

        public int? MarketId { get; set; }

        public DateTime? ListedDate { get; set; }

        public string? CompanyName { get; set; }

        public string? MarketName { get; set; }

        public virtual ICollection<StockInSession> StockInSessions { get; set; } = new List<StockInSession>();

        public virtual ICollection<WatchList> WatchLists { get; set; } = new List<WatchList>();
    }
}
