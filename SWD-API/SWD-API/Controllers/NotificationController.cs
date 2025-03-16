using Microsoft.AspNetCore.Mvc;
using SWD.Service.Interface;
using SWD.Data.DTOs.Notification;
using System;
using System.Threading.Tasks;

namespace SWD_API.Controllers
{
    [Route("api/v{version:apiVersion}/notifications")]
    [ApiVersion("1.0")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Get a paginated list of notifications with optional search and sorting.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetNotifications(
            [FromQuery] string? searchTerm,
            [FromQuery] string? typeFilter,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                
                if (page < 1)
                    return BadRequest(new { Message = "Page number must be greater than 0." });
                if (pageSize < 1 || pageSize > 10)
                    return BadRequest(new { Message = "Page size must be between 1 and 10." });
                var notifications = await _notificationService.GetNotificationsAsync(searchTerm,typeFilter, sortColumn, sortOrder, page, pageSize);
                return Ok(notifications);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving notifications.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get a notification by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationById(int id)
        {
            try
            {
                var notification = await _notificationService.GetNotificationByIdAsync(id);
                return Ok(notification);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Notification with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the notification.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new notification.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid notification data." });

            try
            {
                var createdNotification = await _notificationService.CreateNotificationAsync(dto);
                return Ok(createdNotification);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the notification.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing notification.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNotification(int id, [FromBody] UpdateNotificationDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid notification data." });

            try
            {
                var updatedNotification = await _notificationService.UpdateNotificationAsync(id, dto);
                return Ok(updatedNotification);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Notification with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the notification.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a notification by ID.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int id)
        {
            try
            {
                var result = await _notificationService.DeleteNotificationAsync(id);
                if (result)
                    return NoContent();

                return NotFound(new { Message = $"Notification with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the notification.", Error = ex.Message });
            }
        }
    }
}
