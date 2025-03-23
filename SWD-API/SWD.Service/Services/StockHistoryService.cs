
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD.Data.DTOs.StockHistory;
using SWD.Data.DTOs;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Service.Interface;

namespace SWD.Service.Services
{
    public class StockHistoryService : IStockHistoryService
    {
        private readonly IStockHistoryRepository _stockHistoryRepository;

        public StockHistoryService(IStockHistoryRepository stockHistoryRepository)
        {
            _stockHistoryRepository = stockHistoryRepository;
        }

        public async Task<PageListResponse<StockHistoryDTO>> GetStockHistoriesAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20)
        {
            var stockHistories = await _stockHistoryRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                stockHistories = stockHistories.Where(s => s.StockSymbol.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                stockHistories = sortOrder?.ToLower() == "desc" ?
                    stockHistories.OrderByDescending(GetSortProperty(sortColumn)) :
                    stockHistories.OrderBy(GetSortProperty(sortColumn));
            }

            var totalCount = stockHistories.Count();

            var paginatedStockHistories = stockHistories
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PageListResponse<StockHistoryDTO>
            {
                Items = paginatedStockHistories.Select(MapStockHistoryToDTO).ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                HasNextPage = (page * pageSize) < totalCount,
                HasPreviousPage = page > 1
            };
        }

        public async Task<StockHistoryDTO> GetStockHistoryByIdAsync(int id)
        {
            var stockHistory = await _stockHistoryRepository.GetAsync(s => s.StockHistoryId == id)
                ?? throw new KeyNotFoundException("Stock history not found.");
            return MapStockHistoryToDTO(stockHistory);
        }

        public async Task<StockHistoryDTO> CreateStockHistoryAsync(CreateStockHistoryDTO dto)
        {
            var stockHistory = new StockHistory
            {
                StockSymbol = dto.StockSymbol,
                OutstandingShares = dto.OutstandingShares,
                Osreasonchange = dto.Osreasonchange,
                ListedShares = dto.ListedShares,
                Lsreasonchange = dto.Lsreasonchange,
                RegisteredCapital = dto.RegisteredCapital,
                Rcreasonchange = dto.Rcreasonchange,
                CreatedDate = dto.CreatedDate ?? DateTime.Now
            };

            await _stockHistoryRepository.AddAsync(stockHistory);
            return MapStockHistoryToDTO(stockHistory);
        }

        public async Task<bool> DeleteStockHistoryAsync(int id)
        {
            var stockHistory = await _stockHistoryRepository.GetAsync(s => s.StockHistoryId == id);
            if (stockHistory == null) return false;

            await _stockHistoryRepository.DeleteAsync(stockHistory);
            return true;
        }

        private static Func<StockHistory, object> GetSortProperty(string sortColumn)
        {
            return sortColumn?.ToLower() switch
            {
                "stocksymbol" => stock => stock.StockSymbol,
                "outstandingshares" => stock => stock.OutstandingShares ?? 0,
                "listedshares" => stock => stock.ListedShares ?? 0,
                "registeredcapital" => stock => stock.RegisteredCapital ?? 0,
                "createddate" => stock => stock.CreatedDate,
                _ => stock => stock.StockHistoryId
            };
        }

        private static StockHistoryDTO MapStockHistoryToDTO(StockHistory stockHistory)
        {
            return new StockHistoryDTO
            {
                StockHistoryId = stockHistory.StockHistoryId,
                StockSymbol = stockHistory.StockSymbol,
                OutstandingShares = stockHistory.OutstandingShares,
                Osreasonchange = stockHistory.Osreasonchange,
                ListedShares = stockHistory.ListedShares,
                Lsreasonchange = stockHistory.Lsreasonchange,
                RegisteredCapital = stockHistory.RegisteredCapital,
                Rcreasonchange = stockHistory.Rcreasonchange,
                CreatedDate = stockHistory.CreatedDate
            };
        }
    }
}
