

using System.Threading.Tasks;
using SWD.Data.DTOs.User;
using SWD.Data.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace SWD.Service.Interface
{
    public interface IUserService
    {
        Task<PageListResponse<UserDTO>> GetUsersAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20);
        Task<UserDTO> GetUserByIdAsync(int id);
        Task<UserDTO> CreateUserAsync(CreateUserDTO dto);
        Task<RegisterResponseDTO> RegisterUserAsync(RegisterUserDTO dto);
        Task<UserDTO> UpdateUserAsync(int id, UpdateUserDTO dto);
        Task<bool> DeleteUserAsync(int id);
        Task<bool> ChangeUserRoleToMembers(int userId);
        //Task<string> UpdateUserRoleAsync(int userId, UpdateUserRoleDTO dto);
    }
}
