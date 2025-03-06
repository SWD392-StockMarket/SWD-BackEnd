using SWD.Data.DTOs.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.DTOs.WatchLists
{
    public class WatchListDTO
    {
        public int WatchListId { get; set; }
        public int? UserId { get; set; }
        public string? Label { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastEdited { get; set; }
        public string? Status { get; set; }
        public List<StockDTO>? Stocks { get; set; }
    }
}
