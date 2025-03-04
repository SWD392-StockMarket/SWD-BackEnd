

using SWD.Data.DTOs.User;
using SWD.Data.DTOs;

namespace SWD.Service.Interface
{
    public interface IUserService
    {
        Task<PageListResponse<UserDTO>> GetUsersAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20);
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<UserDTO> CreateUserAsync(CreateUserDTO dto);
        Task<UserDTO> UpdateUserAsync(int id, UpdateUserDTO dto);
        Task<bool> DeleteUserAsync(int id);
        //Task<string> UpdateUserRoleAsync(int userId, UpdateUserRoleDTO dto);
    }
}
