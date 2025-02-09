using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.Entities
{
    public class StockWatchList
    {
        public int StockID { get; set; }
        public Stock Stock { get; set; }

        public int WatchListID { get; set; }
        public WatchList WatchList { get; set; }
    }
}
