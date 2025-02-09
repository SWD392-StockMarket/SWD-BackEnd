using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Repository.Repositories;

namespace SWD_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly IStockRopository _repo;
        public StockController (IStockRopository stockRopository)
        {
            _repo = stockRopository;
        }


        [HttpGet]
        public async Task<ActionResult<ICollection<Stock>>> GetStocks()
        {
            var stocks = await _repo.GetAllStocksAsync(); 
            return Ok(stocks);
        }
    }
}
