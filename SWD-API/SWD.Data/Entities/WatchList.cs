using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.Entities
{
    public class WatchList
    {
        [Key]
        public int WatchListID { get; set; }

        public int UserID { get; set; }
        public User User { get; set; }

        public string Label { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime LastEdited { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }

        public ICollection<StockWatchList> StockWatchLists { get; set; }
    }
}
