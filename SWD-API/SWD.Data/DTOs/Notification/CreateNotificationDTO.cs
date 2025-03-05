using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.DTOs.Notification
{
    public class CreateNotificationDTO
    {
        public int? StaffId { get; set; }
        public string? Title { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? Navigation { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; } = "Active";
    }

}
