using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.Data.DTOs.WatchLists;
using SWD.Service.Interface;

namespace SWD_API.Controllers
{
    [Route("api/watchlists")]
    [ApiController]
    public class WatchListController : ControllerBase
    {
        private readonly IWatchListService _watchListService;

        public WatchListController(IWatchListService watchListService)
        {
            _watchListService = watchListService;
        }

        /// <summary>
        /// Get a paginated list of watchlists with optional search and sorting
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetWatchLists([FromQuery] string? searchTerm, [FromQuery] string? sortColumn,
                                                        [FromQuery] string? sortOrder, [FromQuery] int page = 1,
                                                        [FromQuery] int pageSize = 20)
        {
            try
            {
                var watchLists = await _watchListService.GetWatchListsAsync(searchTerm, sortColumn, sortOrder, page, pageSize);
                return Ok(watchLists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving watchlists.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get a watchlist by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWatchListById(int id)
        {
            try
            {
                var watchList = await _watchListService.GetWatchListByIdAsync(id);
                return Ok(watchList);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Watchlist with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the watchlist.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get watchlists by User ID
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetWatchListsByUserId(int userId)
        {
            try
            {
                var watchLists = await _watchListService.GetWatchListsByUserIdAsync(userId);
                return Ok(watchLists);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving watchlists for the user.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new watchlist
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateWatchList([FromBody] CreateWatchListDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid watchlist data." });

            try
            {
                var createdWatchList = await _watchListService.CreateWatchListAsync(dto);
                return Ok(createdWatchList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the watchlist.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing watchlist
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWatchList(int id, [FromBody] UpdateWatchListDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid watchlist data." });

            try
            {
                var updatedWatchList = await _watchListService.UpdateWatchListAsync(id, dto);
                return Ok(updatedWatchList);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Watchlist with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the watchlist.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a watchlist by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWatchList(int id)
        {
            try
            {
                var result = await _watchListService.DeleteWatchListAsync(id);
                if (result)
                    return NoContent();

                return NotFound(new { Message = $"Watchlist with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the watchlist.", Error = ex.Message });
            }
        }
    }
}
