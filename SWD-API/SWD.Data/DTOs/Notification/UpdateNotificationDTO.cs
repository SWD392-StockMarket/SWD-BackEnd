﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.DTOs.Notification
{
    public class UpdateNotificationDTO
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Navigation { get; set; }
        public string? Type { get; set; }
        public string? Status { get; set; }
        
        public DateTime? ScheduledTime { get; set; }
    }
}
