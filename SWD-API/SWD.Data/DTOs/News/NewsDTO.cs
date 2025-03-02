using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.DTOs.News
{
    public class NewsDTO
    {
        public int NewsId { get; set; }
        public int? StaffId { get; set; }
        
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Type { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastEdited { get; set; }
        public string? Status { get; set; }
        public string? Url { get; set; }
        public string? StaffName { get; set; } // Include staff name if needed
    }

}
