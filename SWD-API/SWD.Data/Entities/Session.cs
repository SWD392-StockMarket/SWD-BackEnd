using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Data.Entities
{
    public class Session
    {
        [Key]
        public int SessionID { get; set; }

        public string SessionType { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string Status { get; set; }

        public ICollection<StockInSession> StockInSessions { get; set; }
    }
}
