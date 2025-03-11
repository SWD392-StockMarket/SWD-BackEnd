using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.DTOs.WatchLists
{
    public class UpdateWatchListDTO
    {
        public string? Label { get; set; }
        public string? Status { get; set; }
        public List<int>? StockIds { get; set; } // Updated list of stock IDs
    }
}
