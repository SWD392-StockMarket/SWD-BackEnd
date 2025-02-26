using SWD.Data.DTOs;
using SWD.Data.DTOs.Stock;
using SWD.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Service.Interface
{
    public interface IStockService
    {
        Task<PageListResponse<StockDTO>> GetStocksAsync(string? searchTerm,
       string? sortColumn,
       string? sortOrder,
       int page = 1,
       int pageSize = 20);
        Task<StockDTO> GetStockById(int id);

        Task<StockDTO> UpdateStock(int id, UpdateStockDTO dto);
        Task<StockDTO> CreateStock(CreateStockDTO dto);
        Task<bool> DeleteStock(int id);
        Task<List<StockHistory>> GetStockHistoryAsync(string stockSymboll);
    }
}
