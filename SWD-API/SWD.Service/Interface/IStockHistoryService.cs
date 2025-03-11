

using SWD.Data.DTOs.StockHistory;
using SWD.Data.DTOs;

namespace SWD.Service.Interface
{
    public interface IStockHistoryService
    {
        Task<PageListResponse<StockHistoryDTO>> GetStockHistoriesAsync(string? stockSymbol, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20);
        Task<StockHistoryDTO> GetStockHistoryByIdAsync(int id);
        Task<StockHistoryDTO> CreateStockHistoryAsync(CreateStockHistoryDTO dto);
        Task<bool> DeleteStockHistoryAsync(int id);
    }
}
