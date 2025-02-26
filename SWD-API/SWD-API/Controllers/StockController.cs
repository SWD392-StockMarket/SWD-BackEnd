using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.Data.DTOs.Stock;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Repository.Repositories;
using SWD.Service.Interface;

namespace SWD_API.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockService _stockService;
        public StockController (IStockService stockService)
        {
            _stockService = stockService;
        }


        /// <summary>
        /// Get a paginated list of stocks with optional search and sorting
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStocks([FromQuery] string? searchTerm, [FromQuery] string? sortColumn, [FromQuery] string? sortOrder, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            try
            {
                var stocks = await _stockService.GetStocksAsync(searchTerm, sortColumn, sortOrder, page, pageSize);
                return Ok(stocks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving stocks.", Error = ex.Message });
            }
        }
        /// <summary>
        /// Get stock by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockById(int id)
        {
            try
            {
                var stock = await _stockService.GetStockById(id);
                return Ok(stock);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Stock with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving stock.", Error = ex.Message });
            }
        }
        /// <summary>
        /// Create a new stock
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid stock data." });

            try
            {
                var createdStock = await _stockService.CreateStock(dto);
                //return CreatedAtAction(nameof(GetStockById), new { id = createdStock.StockId }, createdStock);
                return Ok(createdStock);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating stock.", Error = ex.Message });
            }
        }
        /// <summary>
        /// Update an existing stock
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid stock data." });

            try
            {
                var updatedStock = await _stockService.UpdateStock(id, dto);
                return Ok(updatedStock);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Stock with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating stock.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a stock by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            try
            {
                var result = await _stockService.DeleteStock(id);
                if (result)
                    return NoContent();

                return NotFound(new { Message = $"Stock with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting stock.", Error = ex.Message });
            }
        }
        [HttpGet("history/{stockSymbol}")]
        public async Task<IActionResult> GetStockHistory(string stockSymbol)
        {
            try
            {
                var stockHistory = await _stockService.GetStockHistoryAsync(stockSymbol);
                if (stockHistory == null || stockHistory.Count == 0)
                    return NotFound(new { Message = $"No history found for stock symbol {stockSymbol}." });

                return Ok(stockHistory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving stock history.", Error = ex.Message });
            }
        }
    }
}
