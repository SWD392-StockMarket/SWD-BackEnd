using Microsoft.EntityFrameworkCore;
using SWD.Data.DTOs;
using SWD.Data.DTOs.Stock;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Repository.Repositories;
using SWD.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Service.Services
{
    public class StockService : IStockService
    {
        public readonly IStockRopository _stockRopository;
        public StockService(IStockRopository stockRopository)
        {
            _stockRopository = stockRopository;
        }

        public async Task<StockDTO> CreateStock(CreateStockDTO dto)
        {
            var stock = new Stock
            {
                StockSymbol = dto.StockSymbol,
                CompanyId = dto.CompanyId,
                MarketId = dto.MarketId,
                ListedDate = dto.ListedDate
            };

            // Add the new stock to the database
            _stockRopository.AddAsync(stock);

            // Return the created stock as a DTO
            return new StockDTO
            {
                StockId = stock.StockId,
                StockSymbol = stock.StockSymbol,
                CompanyId = stock.CompanyId,
                MarketId = stock.MarketId,
                ListedDate = stock.ListedDate
            };
        }

        public async Task<bool> DeleteStock(int id)
        {
            var stock = await _stockRopository.GetAsync(s => s.StockId == id);

            if (stock == null)
            {
                throw new KeyNotFoundException("Stock not found.");
            }

            await _stockRopository.DeleteAsync(stock);
            return true;
        }

        public async Task<StockDTO> GetStockById(int id)
        {
            var stock = await _stockRopository.GetAsync(s => s.StockId == id, includeProperties: "Company,Market,StockInSessions,WatchLists") ?? throw new KeyNotFoundException("Stock is not found");
            return new StockDTO
            {
                StockId = stock.StockId,
                CompanyId = stock.CompanyId,
                StockSymbol = stock.StockSymbol,
                MarketId = stock.MarketId,
                ListedDate = stock.ListedDate,
                CompanyName = stock.Company?.CompanyName,
                MarketName = stock.Market?.MarketName,
                StockInSessions = stock.StockInSessions.ToList(),
                WatchLists = stock.WatchLists.ToList()
            };
        }

        public async Task<PageListResponse<StockDTO>> GetStocksAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20)
        {
            var categories = await _stockRopository.GetAllAsync(includeProperties: "Company,Market,StockInSessions,WatchLists");


            // Apply search filter if searchTerm is provided
            // search by stock symbol 
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                categories = categories.Where(c => c.StockSymbol.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Apply sorting if sortOrder is provided
            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                if (sortOrder?.ToLower() == "desc")
                {
                    categories = categories.OrderByDescending(GetSortProperty(sortColumn));
                }
                else
                {
                    categories = categories.OrderBy(GetSortProperty(sortColumn)).ToList();
                }
            }

            var totalCount = categories.Count();


            var paginatedCategpries = categories
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();


            return new PageListResponse<StockDTO>
            {
                Items = MapCategoriesToDTOs(paginatedCategpries),//map to dto
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                HasNextPage = (page * pageSize) < totalCount,
                HasPreviousPage = page > 1
            };
        }

        public async Task<StockDTO> UpdateStock(int id, UpdateStockDTO dto)
        {
            var stock = await _stockRopository.GetAsync(s => s.StockId == id, includeProperties: "Company,Market,StockInSessions,WatchLists");

            if (stock == null)
            {
                throw new KeyNotFoundException("Stock not found.");
            }

            // Update properties from DTO
            stock.CompanyId = dto.CompanyId;
            stock.StockSymbol = dto.StockSymbol;
            stock.MarketId = dto.MarketId;
            stock.ListedDate = dto.ListedDate;

            // Save the changes
            var updatedStock = await _stockRopository.UpdateAsync(stock);

            // Return updated stock as DTO
            return new StockDTO
            {
                StockId = updatedStock.StockId,
                CompanyId = updatedStock.CompanyId,
                StockSymbol = updatedStock.StockSymbol,
                MarketId = updatedStock.MarketId,
                ListedDate = updatedStock.ListedDate,
                CompanyName = updatedStock.Company?.CompanyName,
                MarketName = updatedStock.Market?.MarketName,
                StockInSessions = updatedStock.StockInSessions.ToList(),
                WatchLists = updatedStock.WatchLists.ToList()
            };
        }
        private static Func<Stock, object> GetSortProperty(string SortColumn)
        {
            return SortColumn?.ToLower() switch
            {
                //"company" => stock => stock.Company.CompanyName,
                "stockSymbol" => stock => stock.StockSymbol,
                "listedDate" => stock => stock.ListedDate,
                _ => stock => stock.StockId

            };
        }
        private List<StockDTO> MapCategoriesToDTOs(List<Stock> stocks)
        {
            return stocks.Select(s => new StockDTO
            {
                StockId = s.StockId,
                CompanyId = s.CompanyId,
                StockSymbol = s.StockSymbol,
                MarketId = s.MarketId,
                ListedDate = s.ListedDate,
                CompanyName = s.Company?.CompanyName,
                MarketName = s.Market?.MarketName,
                StockInSessions = s.StockInSessions.ToList(),
                WatchLists = s.WatchLists.ToList()

            }).ToList();
        }
    }
}
