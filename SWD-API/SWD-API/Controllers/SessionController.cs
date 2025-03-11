using Microsoft.AspNetCore.Mvc;
using SWD.Data.DTOs.Session;
using SWD.Service.Interface;


namespace SWD_API.Controllers
{
    [Route("api/v{version:apiVersion}/sessions")]
    [ApiVersion("1.0")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        /// <summary>
        /// Get a paginated list of sessions with optional search and sorting
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetSessions([FromQuery] string? searchTerm, [FromQuery] string? sortColumn,
                                                     [FromQuery] string? sortOrder, [FromQuery] int page = 1,
                                                     [FromQuery] int pageSize = 20)
        {
            try
            {
                var sessions = await _sessionService.GetSessionsAsync(searchTerm, sortColumn, sortOrder, page, pageSize);
                return Ok(sessions);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving sessions.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get a session by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSessionById(int id)
        {
            try
            {
                var session = await _sessionService.GetSessionByIdAsync(id);
                return Ok(session);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Session with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the session.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new session
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid session data." });

            try
            {
                var createdSession = await _sessionService.CreateSessionAsync(dto);
                return Ok(createdSession);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the session.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing session
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSession(int id, [FromBody] UpdateSessionDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid session data." });

            try
            {
                var updatedSession = await _sessionService.UpdateSessionAsync(id, dto);
                return Ok(updatedSession);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Session with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the session.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a session by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            try
            {
                var result = await _sessionService.DeleteSessionAsync(id);
                if (result)
                    return NoContent();

                return NotFound(new { Message = $"Session with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the session.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get all stocks of a session
        /// </summary>
        [HttpGet("{id}/stocks")]
        public async Task<IActionResult> GetSessionStocks(int id)
        {
            try
            {
                var stocks = await _sessionService.GetSessionStocksAsync(id);
                if (stocks == null || stocks.Count == 0)
                    return NotFound(new { Message = $"No stocks found for session with ID {id}." });

                return Ok(stocks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving session stocks.", Error = ex.Message });
            }
        }
    }
}
