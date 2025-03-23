using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.Service.Interface;
namespace SWD_API.Controllers;

    [Route("api/v{version:apiVersion}/user-stats")]
    [ApiVersion("1.0")]
    [ApiController]
    public class UsersStatsController : ControllerBase
    {
        private readonly IUsersStatsService _userStatsService;

        public UsersStatsController(IUsersStatsService userStatsService)
        {
            _userStatsService = userStatsService;
        }

    // ✅ GET /api/user-stats
    [Authorize(Roles = "Admin",AuthenticationSchemes =JwtBearerDefaults.AuthenticationScheme)]
    [HttpGet]
        public async Task<IActionResult> GetUsersStats()
        {
            var stats = await _userStatsService.GetUsersStatsAsync();
            
            if (stats == null)
            {
                return NotFound("User statistics not found.");
            }

            return Ok(stats);
        }
    }
