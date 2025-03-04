
namespace SWD.Data.DTOs.User
{
    public class CreateUserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? SubscriptionStatus { get; set; }
    }
}
