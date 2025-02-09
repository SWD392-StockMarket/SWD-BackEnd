using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.Entities
{
    public class StockHistory
    {
        [Key]
        public int StockHistoryID { get; set; }

        [Required, MaxLength(50)]
        public string StockSymbol { get; set; }

        public long OutstandingShares { get; set; }
        public string OSReasonchange { get; set; }
        public long ListedShares { get; set; }
        public string LSReasonchange { get; set; }
        public decimal RegisteredCapital { get; set; }
        public string RCReasonchange { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
