using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SWD.Data.DTOs.Authentication;
using SWD.Data.Entities;
using SWD.Service.Interface;

namespace SWD_API.Controllers
{
    [Route("api/v{version:apiVersion}/auth")]
    [ApiVersion("1.0")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        
        private readonly UserManager<User> _userManager;
        public AuthController(IAuthService authService, UserManager<User> userManager)
        {
            _authService = authService;
            _userManager = userManager;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("Invalid credentials.");
            }
            // Check if the account is soft-deleted
            if (user.Status == "Deleted") // Adjust this condition based on your actual property
            {
                return Unauthorized("Your account has been deleted. Please contact the admin!");
            }
            
            var token = _authService.GenerateToken(user);
            return Ok(new
            {
                Token = token ,
                userId = user.Id
            });
        }
        
    }
}
