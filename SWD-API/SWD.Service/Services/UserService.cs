using Microsoft.AspNetCore.Identity;
using SWD.Data.DTOs;
using SWD.Data.DTOs.User;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Service.Interface;


namespace SWD.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        
        public UserService(IUserRepository userRepository, UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<PageListResponse<UserDTO>> GetUsersAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20)
        {
            var users = await _userRepository.GetAllAsync();
            
            users = users.Where(u => u.Status == "Active");

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                users = users.Where(u => u.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                users = sortOrder?.ToLower() == "desc"
                    ? users.OrderByDescending(GetSortProperty(sortColumn))
                    : users.OrderBy(GetSortProperty(sortColumn));
            }

            var totalCount = users.Count();

            // Apply pagination
            var paginatedUsers = users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PageListResponse<UserDTO>
            {
                Items = paginatedUsers.Select(MapToDTO).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                HasNextPage = (page * pageSize) < totalCount,
                HasPreviousPage = page > 1
            };
        }

        public async Task<UserDTO> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetAsync(u => u.Id == id)
                        ?? throw new KeyNotFoundException("User not found.");
            return MapToDTO(user);
        }

        public async Task<UserDTO> CreateUserAsync(CreateUserDTO dto)
        {
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if (existingUser != null)
            {
                throw new Exception("Email is already in use.");
            }
            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Status = "Active",
                SubscriptionStatus = dto.SubscriptionStatus,
                CreatedAt = DateTime.UtcNow,
                LastEdited = DateTime.UtcNow
            };

            // await _userRepository.AddAsync(user);
            // Hash password và tạo user
            var result = await _userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
            }

            // Gán role cho user (nếu có)
            await _userManager.AddToRoleAsync(user, "MARKETANALIZER");

            return MapToDTO(user);
        }

        public async Task<UserDTO> UpdateUserAsync(int id, UpdateUserDTO dto)
        {
            var user = await _userRepository.GetAsync(u => u.Id == id)
                        ?? throw new KeyNotFoundException("User not found.");


            user.Email = dto.Email ?? user.Email;
            user.UserName = dto.UserName ?? user.UserName;
            user.SubscriptionStatus = dto.SubscriptionStatus ?? user.SubscriptionStatus;
            user.LastEdited = DateTime.UtcNow;

            var updatedUser = await _userRepository.UpdateAsync(user);
            return MapToDTO(updatedUser);
        }
        
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetAsync(u => u.Id == id);
            if (user == null) return false;
            
            user.Status = "Deleted";
            await _userRepository.UpdateAsync(user);
            return true;
        }

        private static Func<User, object> GetSortProperty(string sortColumn)
        {
            return sortColumn?.ToLower() switch
            {
                "username" => u => u.UserName,
                "email" => u => u.Email,
                "createdat" => u => u.CreatedAt,
                "lastedited" => u => u.LastEdited,
                _ => u => u.Id
            };
        }

        private static UserDTO MapToDTO(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Status = user.Status,
                SubscriptionStatus = user.SubscriptionStatus,
                CreatedAt = user.CreatedAt,
                LastEdited = user.LastEdited
            };
        }
    }
}
