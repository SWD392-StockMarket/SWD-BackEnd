using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.Entities
{
    public class StockInSession
    {
        [Key]
        public int StockInSessionID { get; set; }

        public int StockID { get; set; }
        public Stock Stock { get; set; }

        public int SessionID { get; set; }
        public Session Session { get; set; }

        public DateTime DateTime { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
    }
}
