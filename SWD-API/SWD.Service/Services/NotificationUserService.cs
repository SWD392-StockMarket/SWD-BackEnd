using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD.Data.DTOs;
using SWD.Data.DTOs.Notification;
using SWD.Data.DTOs.NotificationUser;
using SWD.Data.DTOs.User;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Service.Interface;


namespace SWD.Service.Services
{
    public class NotificationUserService : INotificationUserService
    {
        private readonly INotificationUserRepository _notificationUserRepository;

        public NotificationUserService(INotificationUserRepository notificationUserRepository)
        {
            _notificationUserRepository = notificationUserRepository;
        }

        public async Task<PageListResponse<NotificationUserDTO>> GetNotificationUsersAsync(
            string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20)
        {
            var notificationUsers = await _notificationUserRepository.GetAllAsync(includeProperties: "Notification,User");

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                notificationUsers = notificationUsers.Where(nu =>
                    nu.Notification.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    nu.User.UserName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                if (sortOrder?.ToLower() == "desc")
                {
                    notificationUsers = notificationUsers.OrderByDescending(GetSortProperty(sortColumn));
                }
                else
                {
                    notificationUsers = notificationUsers.OrderBy(GetSortProperty(sortColumn)).ToList();
                }
            }

            var totalCount = notificationUsers.Count();

            // Apply pagination
            var paginatedNotificationUsers = notificationUsers
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PageListResponse<NotificationUserDTO>
            {
                Items = MapNotificationUsersToDTOs(paginatedNotificationUsers),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                HasNextPage = (page * pageSize) < totalCount,
                HasPreviousPage = page > 1
            };
        }

        public async Task<NotificationUserDTO> GetNotificationUserByIdAsync(int notificationId, int userId)
        {
            var notificationUser = await _notificationUserRepository.GetAsync(
                nu => nu.NotificationId == notificationId && nu.UserId == userId,
                includeProperties: "Notification,User"
            ) ?? throw new KeyNotFoundException("Notification-User record not found.");

            return MapNotificationUserToDTO(notificationUser);
        }

        public async Task<NotificationUserDTO> CreateNotificationUserAsync(CreateNotificationUserDTO dto)
        {
            var notificationUser = new NotificationUser
            {
                NotificationId = dto.NotificationId,
                UserId = dto.UserId,
                Status = dto.Status
            };

            await _notificationUserRepository.AddAsync(notificationUser);
            return MapNotificationUserToDTO(notificationUser);
        }

        public async Task<NotificationUserDTO> UpdateNotificationUserAsync(int notificationId, int userId, UpdateNotificationUserDTO dto)
        {
            var notificationUser = await _notificationUserRepository.GetAsync(
                nu => nu.NotificationId == notificationId && nu.UserId == userId
            ) ?? throw new KeyNotFoundException("Notification-User record not found.");

            notificationUser.Status = dto.Status;

            var updatedNotificationUser = await _notificationUserRepository.UpdateAsync(notificationUser);
            return MapNotificationUserToDTO(updatedNotificationUser);
        }

        public async Task<bool> DeleteNotificationUserAsync(int notificationId, int userId)
        {
            var notificationUser = await _notificationUserRepository.GetAsync(
                nu => nu.NotificationId == notificationId && nu.UserId == userId
            );
            if (notificationUser == null) return false;

            await _notificationUserRepository.DeleteAsync(notificationUser);
            return true;
        }

        private static Func<NotificationUser, object> GetSortProperty(string sortColumn)
        {
            return sortColumn?.ToLower() switch
            {
                "status" => nu => nu.Status ?? "",
                "notificationtitle" => nu => nu.Notification.Title,
                "username" => nu => nu.User.UserName,
                _ => nu => nu.NotificationId
            };
        }

        private static NotificationUserDTO MapNotificationUserToDTO(NotificationUser notificationUser)
        {
            return new NotificationUserDTO
            {
                NotificationId = notificationUser.NotificationId,
                UserId = notificationUser.UserId,
                Status = notificationUser.Status,
                Notification = new NotificationDTO
                {
                    NotificationId = notificationUser.Notification.NotificationId,
                    Title = notificationUser.Notification.Title,
                    Content = notificationUser.Notification.Content,
                    CreatedDate = notificationUser.Notification.UpdatedDate
                },
                User = new UserDTO
                {
                    Id = notificationUser.User.Id,
                    UserName = notificationUser.User.UserName,
                    Email = notificationUser.User.Email,
                    Status = notificationUser.User.Status,
                    CreatedAt = notificationUser.User.CreatedAt,
                    LastEdited = notificationUser.User.LastEdited,
                    SubscriptionStatus = notificationUser.User.SubscriptionStatus
                }
            };
        }

        private List<NotificationUserDTO> MapNotificationUsersToDTOs(List<NotificationUser> notificationUsers)
        {
            return notificationUsers.Select(MapNotificationUserToDTO).ToList();
        }
    }
}
