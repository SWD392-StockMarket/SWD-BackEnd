using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.DTOs.NotificationUser
{
    public class CreateNotificationUserDTO
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string? Status { get; set; }
    }
}
