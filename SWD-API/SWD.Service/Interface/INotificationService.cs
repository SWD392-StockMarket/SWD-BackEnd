using SWD.Data.DTOs.Notification;
using SWD.Data.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWD.Data.Entities;

namespace SWD.Service.Interface
{
    public interface INotificationService
    {
        Task<PageListResponse<NotificationDTO>> GetNotificationsAsync(
            string? searchTerm,string? typeFilter, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20);

        Task<NotificationDTO> GetNotificationByIdAsync(int id);

        Task<NotificationDTO> CreateNotificationAsync(CreateNotificationDTO dto);

        Task<NotificationDTO> UpdateNotificationAsync(int id, UpdateNotificationDTO dto);

        Task<bool> DeleteNotificationAsync(int id);
        Task SendNotificationAsync(Notification notification);
    }
}
