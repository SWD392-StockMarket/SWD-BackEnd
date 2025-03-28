using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.Data.DTOs.User;
using SWD.Service.Interface;

namespace SWD_API.Controllers
{
    [Route("api/v{version:apiVersion}/users")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Get a paginated list of users with optional search and sorting
        /// </summary>
        
        [Authorize(Roles = "Admin",AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetUsers([FromQuery] string? searchTerm, [FromQuery] string? sortColumn, [FromQuery] string? sortOrder, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var users = await _userService.GetUsersAsync(searchTerm, sortColumn, sortOrder, page, pageSize);
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving users.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                return Ok(user);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the user.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        [Authorize(Roles = "Admin",AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid user data." });

            try
            {
                var createdUser = await _userService.CreateUserAsync(dto);
                return Ok(createdUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the user.", Error = ex.Message });
            }
        }
        
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid user data." });

            try
            {
                var response = await _userService.RegisterUserAsync(dto);
                return Ok(new
                {
                    User = response.User,
                    Token = response.Token,
                    userId = response.UserId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while register the user.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        [Authorize(Roles = "Admin",AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid user data." });

            try
            {
                var updatedUser = await _userService.UpdateUserAsync(id, dto);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"User with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the user.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a user by ID
        /// </summary>
        [Authorize(Roles = "Admin",AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(id);
                if (result)
                    return NoContent();

                return NotFound(new { Message = $"User with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the user.", Error = ex.Message });
            }
        }
        [HttpPost("{userId}/change-role")]
        public async Task<IActionResult> ChangeUserRoleToMembers(int userId)
        {
            try
            {
                await _userService.ChangeUserRoleToMembers(userId);
                return Ok(new { message = "User role updated to MEMBERS successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        //[Authorize(Roles ="Admin")]
        //[HttpPut("{userId}/role")]
        //public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] UpdateUserRoleDTO dto)
        //{
        //    try
        //    {
        //        var result = await _userService.UpdateUserRoleAsync(userId, dto);
        //        return Ok(new { Message = result });
        //    }
        //    catch (KeyNotFoundException ex)
        //    {
        //        return NotFound(new { Message = ex.Message });
        //    }
        //    catch (ArgumentException ex)
        //    {
        //        return BadRequest(new { Message = ex.Message });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { Message = "An error occurred.", Error = ex.Message });
        //    }
        //}
    }
}
