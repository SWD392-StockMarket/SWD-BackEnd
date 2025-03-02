using Microsoft.AspNetCore.Mvc;
using SWD.Data.DTOs.News;
using SWD.Service.Interface;

namespace SWD_API.Controllers
{
    [Route("api/news")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// Get a paginated list of news with optional search and sorting
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetNews([FromQuery] string? searchTerm, [FromQuery] string? sortColumn,
                                                 [FromQuery] string? sortOrder, [FromQuery] int page = 1,
                                                 [FromQuery] int pageSize = 20)
        {
            try
            {
                var newsList = await _newsService.GetNewsAsync(searchTerm, sortColumn, sortOrder, page, pageSize);
                return Ok(newsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving news.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get a news article by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNewsById(int id)
        {
            try
            {
                var news = await _newsService.GetNewsByIdAsync(id);
                return Ok(news);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"News with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the news.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new news article
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateNews([FromBody] CreateNewsDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid news data." });

            try
            {
                var createdNews = await _newsService.CreateNewsAsync(dto);
                return Ok(createdNews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the news.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing news article
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(int id, [FromBody] UpdateNewsDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid news data." });

            try
            {
                var updatedNews = await _newsService.UpdateNewsAsync(id, dto);
                return Ok(updatedNews);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"News with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the news.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a news article by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            try
            {
                var result = await _newsService.DeleteNewsAsync(id);
                if (result)
                    return NoContent();
        
                return NotFound(new { Message = $"News with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the news.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get news articles by type
        /// </summary>
        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetNewsByType(string type)
        {
            try
            {
                var newsList = await _newsService.GetNewsByTypeAsync(type);
                return Ok(newsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving news by type.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get news articles by status
        /// </summary>
        [HttpGet("status/{status}")]
        public async Task<IActionResult> GetNewsByStatus(string status)
        {
            try
            {
                var newsList = await _newsService.GetNewsByStatusAsync(status);
                return Ok(newsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving news by status.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get the author of a news article
        /// </summary>
        [HttpGet("{id}/author")]
        public async Task<IActionResult> GetNewsAuthor(int id)
        {
            try
            {
                var author = await _newsService.GetNewsAuthorAsync(id);
                return Ok(author);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Author for news ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the news author.", Error = ex.Message });
            }
        }
    }
}
