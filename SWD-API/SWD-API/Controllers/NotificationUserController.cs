using Microsoft.AspNetCore.Mvc;
using SWD.Data.DTOs.NotificationUser;
using SWD.Service.Interface;

namespace SWD_API.Controllers
{
    [Route("api/v{version:apiVersion}/notification-users")]
    [ApiVersion("1.0")]
    [ApiController]
    public class NotificationUserController : ControllerBase
    {
        private readonly INotificationUserService _notificationUserService;

        public NotificationUserController(INotificationUserService notificationUserService)
        {
            _notificationUserService = notificationUserService;
        }

        /// <summary>
        /// Get a paginated list of notification-user records with optional search and sorting
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetNotificationUsers([FromQuery] string? searchTerm, [FromQuery] string? sortColumn,
                                                              [FromQuery] string? sortOrder, [FromQuery] int page = 1,
                                                              [FromQuery] int pageSize = 20)
        {
            try
            {
                var notificationUsers = await _notificationUserService.GetNotificationUsersAsync(searchTerm, sortColumn, sortOrder, page, pageSize);
                return Ok(notificationUsers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving notification users.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get a specific notification-user record by NotificationId and UserId
        /// </summary>
        [HttpGet("{notificationId}/{userId}")]
        public async Task<IActionResult> GetNotificationUserById(int notificationId, int userId)
        {
            try
            {
                var notificationUser = await _notificationUserService.GetNotificationUserByIdAsync(notificationId, userId);
                return Ok(notificationUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Notification-User record not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the notification-user record.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new notification-user record
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateNotificationUser([FromBody] CreateNotificationUserDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid notification-user data." });

            try
            {
                var createdNotificationUser = await _notificationUserService.CreateNotificationUserAsync(dto);
                return Ok(createdNotificationUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the notification-user record.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing notification-user record
        /// </summary>
        [HttpPut("{notificationId}/{userId}")]
        public async Task<IActionResult> UpdateNotificationUser(int notificationId, int userId, [FromBody] UpdateNotificationUserDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid notification-user data." });

            try
            {
                var updatedNotificationUser = await _notificationUserService.UpdateNotificationUserAsync(notificationId, userId, dto);
                return Ok(updatedNotificationUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Notification-User record not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the notification-user record.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a notification-user record by NotificationId and UserId
        /// </summary>
        [HttpDelete("{notificationId}/{userId}")]
        public async Task<IActionResult> DeleteNotificationUser(int notificationId, int userId)
        {
            try
            {
                var result = await _notificationUserService.DeleteNotificationUserAsync(notificationId, userId);
                if (result)
                    return NoContent();

                return NotFound(new { Message = "Notification-User record not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the notification-user record.", Error = ex.Message });
            }
        }
    }
}
