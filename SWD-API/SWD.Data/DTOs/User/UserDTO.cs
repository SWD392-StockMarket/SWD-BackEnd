

namespace SWD.Data.DTOs.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastEdited { get; set; }
        public string? SubscriptionStatus { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
