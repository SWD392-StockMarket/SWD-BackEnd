using SWD.Data.DTOs.Notification;
using SWD.Data.DTOs.User;


namespace SWD.Data.DTOs.NotificationUser
{
    public class NotificationUserDTO
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string? Status { get; set; }
        public NotificationDTO Notification { get; set; } = null!;
        public UserDTO User { get; set; } = null!;
    }
}
