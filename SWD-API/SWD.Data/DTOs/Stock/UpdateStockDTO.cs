using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.DTOs.Stock
{
    public class UpdateStockDTO
    {
        public string StockSymbol { get; set; } = null!;

        public int? CompanyId { get; set; }

        public int? MarketId { get; set; }

        public DateTime? ListedDate { get; set; }
    }
}
