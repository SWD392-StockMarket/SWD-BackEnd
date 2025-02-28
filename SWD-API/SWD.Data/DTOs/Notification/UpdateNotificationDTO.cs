using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.DTOs.Notification
{
    public class UpdateNotificationDTO
    {
        public string? Content { get; set; }
        public string? Navigation { get; set; }
        public string? Type { get; set; }
    }
}
