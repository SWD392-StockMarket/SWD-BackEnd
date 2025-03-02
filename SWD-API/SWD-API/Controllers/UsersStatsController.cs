using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SWD.Service.Interface;
namespace SWD_API.Controllers;

    [Route("api/user-stats")]
    [ApiController]
    public class UsersStatsController : ControllerBase
    {
        private readonly IUsersStatsService _userStatsService;

        public UsersStatsController(IUsersStatsService userStatsService)
        {
            _userStatsService = userStatsService;
        }

        // ✅ GET /api/user-stats
        [Authorize (Roles = "Admin")]
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
