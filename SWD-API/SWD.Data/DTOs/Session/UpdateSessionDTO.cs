

namespace SWD.Data.DTOs.Session
{
    public class UpdateSessionDTO
    {
        public string? SessionType { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Status { get; set; }
    }
}
