using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.Entities
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required, MaxLength(255)]
        public string Username { get; set; }

        [Required, MaxLength(255)]
        public string Password { get; set; }

        [Required, MaxLength(255)]
        public string Email { get; set; }

        [Required, MaxLength(50)]
        public string Role { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime LastEdited { get; set; } = DateTime.UtcNow;

        [MaxLength(50)]
        public string SubscriptionStatus { get; set; }

        public ICollection<WatchList> WatchLists { get; set; }
    }
}
