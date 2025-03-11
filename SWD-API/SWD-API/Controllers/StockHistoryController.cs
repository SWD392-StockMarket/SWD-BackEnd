using Microsoft.AspNetCore.Mvc;
using SWD.Service.Interface;
using SWD.Data.DTOs.StockHistory;


namespace SWD_API.Controllers
{
    [Route("api/v{version:apiVersion}/stock-histories")]
    [ApiVersion("1.0")]
    [ApiController]
    public class StockHistoryController : ControllerBase
    {
        private readonly IStockHistoryService _stockHistoryService;

        public StockHistoryController(IStockHistoryService stockHistoryService)
        {
            _stockHistoryService = stockHistoryService;
        }

        /// <summary>
        /// Get a paginated list of stock history records with optional filtering and sorting
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStockHistories(
            [FromQuery] string? stockSymbol,
            [FromQuery] string? sortColumn,
            [FromQuery] string? sortOrder,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            try
            {
                var stockHistories = await _stockHistoryService.GetStockHistoriesAsync(stockSymbol, sortColumn, sortOrder, page, pageSize);
                return Ok(stockHistories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving stock histories.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get a stock history record by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockHistoryById(int id)
        {
            try
            {
                var stockHistory = await _stockHistoryService.GetStockHistoryByIdAsync(id);
                return Ok(stockHistory);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Stock history with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving stock history.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new stock history record
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateStockHistory([FromBody] CreateStockHistoryDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid stock history data." });

            try
            {
                var createdStockHistory = await _stockHistoryService.CreateStockHistoryAsync(dto);
                return Ok(createdStockHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating stock history.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a stock history record by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStockHistory(int id)
        {
            try
            {
                var result = await _stockHistoryService.DeleteStockHistoryAsync(id);
                if (result)
                    return NoContent();

                return NotFound(new { Message = $"Stock history with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting stock history.", Error = ex.Message });
            }
        }
    }
}
