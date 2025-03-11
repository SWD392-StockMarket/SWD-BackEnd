
namespace SWD.Data.DTOs.Session
{
    public class SessionDTO
    {
        public int SessionId { get; set; }
        public string? SessionType { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public string? Status { get; set; }
    }
}
