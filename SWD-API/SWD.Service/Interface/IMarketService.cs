using System.Collections.Generic;
using System.Threading.Tasks;
using SWD.Data.DTOs.Stock;
using SWD.Data.DTOs;
using SWD.Data.DTOs.Market;


namespace SWD.Service.Interface
{
    public interface IMarketService
    {
        Task<PageListResponse<MarketDTO>> GetMarketsAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20);
        Task<MarketDTO> GetMarketByIdAsync(int id);
        Task<MarketDTO> CreateMarketAsync(CreateMarketDTO dto);
        Task<MarketDTO> UpdateMarketAsync(int id, UpdateMarketDTO dto);
        Task<bool> DeleteMarketAsync(int id);
        Task<List<StockDTO>> GetMarketStocksAsync(int marketId);
    }
}
