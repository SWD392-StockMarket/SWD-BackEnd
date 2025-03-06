using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.DTOs.WatchLists
{
    public class CreateWatchListDTO
    {
        public int? UserId { get; set; }
        public string? Label { get; set; }
        public string? Status { get; set; }
        public List<int>? StockIds { get; set; } // List of stock IDs to add
    }
}
