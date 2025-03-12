namespace SWD.Data.DTOs.User;

public class RegisterResponseDTO
{
    public UserDTO User { get; set; }
    public string Token { get; set; }
    public int UserId { get; set; }
}