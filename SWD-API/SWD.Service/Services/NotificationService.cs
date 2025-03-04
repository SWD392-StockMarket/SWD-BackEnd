using System.Linq.Expressions;
using SWD.Data.Entities;
using SWD.Data.DTOs;
using SWD.Data.DTOs.Notification;
using SWD.Repository.Interface;
using SWD.Service.Interface;
using System.Linq;


namespace SWD.Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<PageListResponse<NotificationDTO>> GetNotificationsAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20)
        {
            var notifications = await _notificationRepository.GetAllAsync();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                notifications = notifications.Where(n => n.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                notifications = sortOrder?.ToLower() == "desc"
                    ? notifications.OrderByDescending(GetSortProperty(sortColumn))
                    : notifications.OrderBy(GetSortProperty(sortColumn));
            }

            var totalCount = notifications.Count();

            // Apply pagination
            var paginatedNotifications = notifications
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PageListResponse<NotificationDTO>
            {
                Items = paginatedNotifications.Select(MapToDTO).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                HasNextPage = (page * pageSize) < totalCount,
                HasPreviousPage = page > 1
            };
        }

        public async Task<NotificationDTO> GetNotificationByIdAsync(int id)
        {
            var notification = await _notificationRepository.GetAsync(n => n.NotificationId == id)
                                ?? throw new KeyNotFoundException("Notification not found.");
            return MapToDTO(notification);
        }

        public async Task<NotificationDTO> CreateNotificationAsync(CreateNotificationDTO dto)
        {
            var notification = new Notification
            {
                StaffId = dto.StaffId,
                Title = dto.Title,
                Content = dto.Content,
                Navigation = dto.Navigation,
                Type = dto.Type,
                CreatedDate = DateTime.UtcNow,
                // UpdatedDate = DateTime.UtcNow,
                Status = dto.Status,
            };

            await _notificationRepository.AddAsync(notification);
            return MapToDTO(notification);
        }

        public async Task<NotificationDTO> UpdateNotificationAsync(int id, UpdateNotificationDTO dto)
        {
            var notification = await _notificationRepository.GetAsync(n => n.NotificationId == id)
                                ?? throw new KeyNotFoundException("Notification not found.");
            
            notification.Title = dto.Title ?? notification.Title;
            notification.Content = dto.Content ?? notification.Content;
            notification.Navigation = dto.Navigation ?? notification.Navigation;
            notification.Type = dto.Type ?? notification.Type;
            notification.UpdatedDate = DateTime.UtcNow;

            var updatedNotification = await _notificationRepository.UpdateAsync(notification);
            return MapToDTO(updatedNotification);
        }

        public async Task<bool> DeleteNotificationAsync(int id)
        {
            var notification = await _notificationRepository.GetAsync(n => n.NotificationId == id);
            if (notification == null) return false;
            
            notification.Status = "Deleted";
            await _notificationRepository.UpdateAsync(notification);
            return true;
        }

        

        private static Func<Notification, object> GetSortProperty(string SortColumn)
        {
            return SortColumn?.ToLower() switch
            {
                "content" => n => n.Content,
                "createddate" => n => n.CreatedDate,
                "updateddate" => n => n.UpdatedDate,
                _ => n => n.NotificationId

            };
        }

        private static NotificationDTO MapToDTO(Notification notification)
        {
            return new NotificationDTO
            {
                NotificationId = notification.NotificationId,
                StaffId = notification.StaffId,
                Title = notification.Title,
                Content = notification.Content,
                Navigation = notification.Navigation,
                Type = notification.Type,
                CreatedDate = notification.CreatedDate,
                UpdatedDate = notification.UpdatedDate,
                Status = notification.Status
            };
        }
    }
}
