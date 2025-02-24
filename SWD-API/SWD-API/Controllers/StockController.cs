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
            var stocks = await _stockService.GetStocksAsync(searchTerm, sortColumn, sortOrder, page, pageSize);
            return Ok(stocks);
        }
        /// <summary>
        /// Get stock by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStockById(int id)
        {
            var stock = await _stockService.GetStockById(id);
            return Ok(stock);
        }
        /// <summary>
        /// Create a new stock
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateStock([FromBody] CreateStockDTO dto)
        {
            var createdStock = await _stockService.CreateStock(dto);
            return CreatedAtAction(nameof(GetStockById), new { id = createdStock.StockId }, createdStock);
        }
        /// <summary>
        /// Update an existing stock
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockDTO dto)
        {
            var updatedStock = await _stockService.UpdateStock(id, dto);
            return Ok(updatedStock);
        }

        /// <summary>
        /// Delete a stock by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var result = await _stockService.DeleteStock(id);
            if (result)
            {
                return NoContent();
            }
            return NotFound("Stock not found");
        }
    }
}
