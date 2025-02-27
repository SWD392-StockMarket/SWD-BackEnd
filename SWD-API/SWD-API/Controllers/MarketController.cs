using Microsoft.AspNetCore.Mvc;
using SWD.Data.DTOs.Market;
using SWD.Service.Interface;


namespace SWD_API.Controllers
{
    [Route("api/market")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private readonly IMarketService _marketService;

        public MarketController(IMarketService marketService)
        {
            _marketService = marketService;
        }

        /// <summary>
        /// Get a paginated list of markets with optional search and sorting
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetMarkets([FromQuery] string? searchTerm, [FromQuery] string? sortColumn,
                                                    [FromQuery] string? sortOrder, [FromQuery] int page = 1,
                                                    [FromQuery] int pageSize = 20)
        {
            try
            {
                var markets = await _marketService.GetMarketsAsync(searchTerm, sortColumn, sortOrder, page, pageSize);
                return Ok(markets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving markets.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get a market by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMarketById(int id)
        {
            try
            {
                var market = await _marketService.GetMarketByIdAsync(id);
                return Ok(market);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Market with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the market.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new market
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateMarket([FromBody] CreateMarketDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid market data." });

            try
            {
                var createdMarket = await _marketService.CreateMarketAsync(dto);
                return Ok(createdMarket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the market.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing market
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMarket(int id, [FromBody] UpdateMarketDTO dto)
        {
            if (dto == null)
                return BadRequest(new { Message = "Invalid market data." });

            try
            {
                var updatedMarket = await _marketService.UpdateMarketAsync(id, dto);
                return Ok(updatedMarket);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = $"Market with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the market.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a market by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMarket(int id)
        {
            try
            {
                var result = await _marketService.DeleteMarketAsync(id);
                if (result)
                    return NoContent();

                return NotFound(new { Message = $"Market with ID {id} not found." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while deleting the market.", Error = ex.Message });
            }
        }

        /// <summary>
        /// Get all stocks of a market
        /// </summary>
        [HttpGet("{id}/stocks")]
        public async Task<IActionResult> GetMarketStocks(int id)
        {
            try
            {
                var stocks = await _marketService.GetMarketStocksAsync(id);
                if (stocks == null || stocks.Count == 0)
                    return NotFound(new { Message = $"No stocks found for market with ID {id}." });

                return Ok(stocks);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving market stocks.", Error = ex.Message });
            }
        }
    }
}
