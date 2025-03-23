

using System.Threading.Tasks;
using SWD.Data.DTOs.NotificationUser;
using SWD.Data.DTOs;

namespace SWD.Service.Interface
{
    public interface INotificationUserService
    {
        Task<PageListResponse<NotificationUserDTO>> GetNotificationUsersAsync(
            string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20);

        Task<NotificationUserDTO> GetNotificationUserByIdAsync(int notificationId, int userId);

        Task<NotificationUserDTO> CreateNotificationUserAsync(CreateNotificationUserDTO dto);

        Task<NotificationUserDTO> UpdateNotificationUserAsync(int notificationId, int userId, UpdateNotificationUserDTO dto);

        Task<bool> DeleteNotificationUserAsync(int notificationId, int userId);
    }
}
