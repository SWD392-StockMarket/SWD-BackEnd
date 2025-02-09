using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.Entities
{
    public class Stock
    {
        [Key]
        public int StockID { get; set; }

        [Required, MaxLength(50)]
        public string StockSymbol { get; set; }

        public int CompanyID { get; set; }
        public Company Company { get; set; }

        public int MarketID { get; set; }
        public Market Market { get; set; }

        public DateTime? ListedDate { get; set; }

        public ICollection<StockInSession> StockInSessions { get; set; }
        public ICollection<StockWatchList> StockWatchLists { get; set; }
    }
}
