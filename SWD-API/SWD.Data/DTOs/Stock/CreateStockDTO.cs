using SWD.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.DTOs.Stock
{
    public class CreateStockDTO
    {

        public int? CompanyId { get; set; }
        public string StockSymbol { get; set; } = string.Empty;

        public int? MarketId { get; set; }

        public DateTime? ListedDate { get; set; }

        
    }
}
