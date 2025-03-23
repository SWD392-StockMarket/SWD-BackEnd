using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FirebaseAdmin.Messaging;
using SWD.Data.Entities;
using SWD.Data.DTOs;
using SWD.Data.DTOs.Notification;
using SWD.Repository.Interface;
using SWD.Service.Interface;
using System.Linq;
using System.Threading.Tasks;

// Alias to avoid ambiguity
using SWDNotification = SWD.Data.Entities.Notification;
using FCMNotification = FirebaseAdmin.Messaging.Notification;

namespace SWD.Service.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IDeviceTokenRepository _deviceTokenRepository;

        public NotificationService(INotificationRepository notificationRepository, IDeviceTokenRepository deviceTokenRepository)
        {
            _notificationRepository = notificationRepository;
            _deviceTokenRepository = deviceTokenRepository;
        }
        //
        // public async Task<PageListResponse<NotificationDTO>> GetNotificationsAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20)
        // {
        //     var notifications = await _notificationRepository.GetAllAsync();
        //
        //     // Apply search filter
        //     if (!string.IsNullOrWhiteSpace(searchTerm))
        //     {
        //         notifications = notifications.Where(n => n.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        //     }
        //
        //     // Apply sorting
        //     if (!string.IsNullOrWhiteSpace(sortColumn))
        //     {
        //         notifications = sortOrder?.ToLower() == "desc"
        //             ? notifications.OrderByDescending(GetSortProperty(sortColumn))
        //             : notifications.OrderBy(GetSortProperty(sortColumn));
        //     }
        //
        //     var totalCount = notifications.Count();
        //
        //     // Apply pagination
        //     var paginatedNotifications = notifications
        //         .Skip((page - 1) * pageSize)
        //         .Take(pageSize)
        //         .ToList();
        //
        //     return new PageListResponse<NotificationDTO>
        //     {
        //         Items = paginatedNotifications.Select(MapToDTO).ToList(),
        //         Page = page,
        //         PageSize = pageSize,
        //         TotalCount = totalCount,
        //         HasNextPage = (page * pageSize) < totalCount,
        //         HasPreviousPage = page > 1
        //     };
        // }
        
        public async Task<PageListResponse<NotificationDTO>> GetNotificationsAsync(
            string? searchTerm, 
            string? typeFilter, 
            string? sortColumn, 
            string? sortOrder, 
            int page = 1, 
            int pageSize = 20)
        {
            var notifications = await _notificationRepository.GetAllAsync(
                n => n.Status != "Deleted" 
                     && (typeFilter == null || n.Type == typeFilter)
                     && (string.IsNullOrWhiteSpace(searchTerm) 
                         || n.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) 
                         || n.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            );
            

            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                notifications = sortOrder?.ToLower() == "desc"
                    ? notifications.OrderByDescending(GetSortProperty(sortColumn))
                    : notifications.OrderBy(GetSortProperty(sortColumn));
            }

            var totalCount = notifications.Count();
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
            
            var notification = new SWDNotification
            {
                StaffId = dto.StaffId,
                Title = dto.Title,
                Content = dto.Content ?? string.Empty,
                Navigation = dto.Navigation,
                Type = dto.Type,
                CreatedDate = DateTime.UtcNow.AddHours(7),
                ScheduledTime = dto.ScheduledTime,
                Status = dto.ScheduledTime.HasValue ? "Pending" : "Active"
            };
            
            if (notification.Status == "Active" && !notification.ScheduledTime.HasValue)
            {
                notification.ScheduledTime = DateTime.UtcNow.AddHours(7);
            }

            await _notificationRepository.AddAsync(notification);

            // Send immediately if Active
            if (notification.Status == "Active")
            {
                await SendNotificationAsync(notification);
            }

            return MapToDTO(notification);
        }

        public async Task<NotificationDTO> UpdateNotificationAsync(int id, UpdateNotificationDTO dto)
        {
            var notification = await _notificationRepository.GetAsync(n => n.NotificationId == id)
                                ?? throw new KeyNotFoundException("Notification not found.");
            
            // Store the original status for comparison
            var originalStatus = notification.Status;
            
            notification.Title = dto.Title ?? notification.Title;
            notification.Content = dto.Content ?? notification.Content;
            notification.Navigation = dto.Navigation ?? notification.Navigation;
            notification.Type = dto.Type ?? notification.Type;
            notification.UpdatedDate = DateTime.UtcNow.AddHours(7);
            notification.Status = dto.Status ?? notification.Status;
            notification.ScheduledTime = dto.ScheduledTime ?? notification.ScheduledTime;
            
            // Handle status transition logic
            if (notification.Status == "Active")
            {
                // If status is Active and it wasn't before, send immediately
                if (originalStatus != "Active")
                {
                    await SendNotificationAsync(notification);
                    
                    // Set ScheduledTime to now if it was null (for consistency with Create)
                    if (!notification.ScheduledTime.HasValue)
                    {
                        notification.ScheduledTime = DateTime.UtcNow.AddHours(7);
                    }
                }
                // If ScheduledTime is still set but status is Active, clear it (optional, based on your logic)
                else if (notification.ScheduledTime.HasValue && dto.ScheduledTime == null)
                {
                    notification.ScheduledTime = DateTime.UtcNow.AddHours(7); // Or null, depending on your preference
                    await SendNotificationAsync(notification);
                }
            }
            else if (notification.Status == "Pending" && notification.ScheduledTime == null)
            {
                // If status is Pending but no ScheduledTime, it’s invalid—throw or set a default
                throw new ArgumentException("Pending notifications must have a ScheduledTime.");
            }

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

        private static Func<SWDNotification, object> GetSortProperty(string SortColumn)
        {
            return SortColumn?.ToLower() switch
            {
                "content" => n => n.Content,
                "createddate" => n => n.CreatedDate,
                "updateddate" => n => n.UpdatedDate,
                _ => n => n.NotificationId
            };
        }

        public async Task SendNotificationAsync(SWDNotification notification)
        {
            var fcmTokens = await _deviceTokenRepository.GetDeviceToken();
            try
            {
                // Create the message payload
                var message = new Message
                {
                    Notification = new FCMNotification
                    {
                        Title = notification.Title,
                        Body = notification.Content
                    },
                    Token = fcmTokens.FCMToken // Sending to multiple device tokens
                };
            
                // Send the notification
                var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            
                Console.WriteLine($"FCM Response: {response} messages were sent successfully.");
                Console.WriteLine($"FCM Notification Sent at {DateTime.UtcNow.AddHours(7)}");
            
                notification.Status = "Active";
                await _notificationRepository.UpdateAsync(notification);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message} at {DateTime.UtcNow.AddHours(7)}");
                notification.Status = "Failed";
                await _notificationRepository.UpdateAsync(notification);
            }
        }

        private static NotificationDTO MapToDTO(SWDNotification notification)
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
                ScheduledTime = notification.ScheduledTime,
                Status = notification.Status
            };
        }
    }
}

// try
// {
//     var message = new Message
//     {
//         Notification = new FCMNotification
//         {
//             Title = notification.Title,
//             Body = notification.Content
//         },
//         Topic = "users"
//     };
//     string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
//     
//     Console.WriteLine($"FCM Response: {response}");
//     Console.WriteLine($"FCM Notification Sent: {response} at {DateTime.UtcNow.AddHours(7)}");
//     notification.Status = "Active";
//     await _notificationRepository.UpdateAsync(notification);
// }
// catch (Exception ex)
// {
//     Console.WriteLine($"Error sending notification: {ex.Message} at {DateTime.UtcNow.AddHours(7)}");
//     notification.Status = "Failed";
//     await _notificationRepository.UpdateAsync(notification);
// }
