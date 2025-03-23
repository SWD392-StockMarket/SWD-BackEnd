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
        private readonly IDeviceTokenService _deviceTokenService;
        
        private readonly UserManager<User> _userManager;
        public AuthController(IAuthService authService, UserManager<User> userManager, IDeviceTokenService deviceTokenService)
        {
            _authService = authService;
            _userManager = userManager;
            _deviceTokenService = deviceTokenService;
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
            if (user.Status == "Deleted") 
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

        [HttpPut("{id}/{fcmToken}")]
        public async Task<IActionResult> SaveFCMToken(int id ,string fcmToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id.ToString());
                if (user != null)
                {
                   await _deviceTokenService.CreateDeviceToken(id, fcmToken);
                   return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
