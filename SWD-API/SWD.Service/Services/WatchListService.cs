using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD.Data.DTOs;
using SWD.Data.DTOs.Stock;
using SWD.Data.DTOs.WatchLists;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Service.Interface;

namespace SWD.Service.Services
{
    public class WatchListService : IWatchListService
    {
        
            private readonly IWatchListRepository _watchListRepository;
            private readonly IStockRopository _stockRepository;


            public WatchListService(IWatchListRepository watchListRepository, IStockRopository stockRepository)
            {
                _watchListRepository = watchListRepository;
                _stockRepository = stockRepository;
            }
            
            public async Task<WatchListDTO> AddStockToWatchListAsync(int watchListId, int stockId)
            {
                var stock =await _stockRepository.GetAsync(s => s.StockId == stockId);
                
                var watchList =await _watchListRepository.GetAsync(w => w.WatchListId == watchListId);

                watchList.Stocks.Add(stock);

                await _watchListRepository.UpdateAsync(watchList);
                
                return MapWatchListToDTO(watchList);
            }

            public async Task<PageListResponse<WatchListDTO>> GetWatchListsAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20)
            {
                var watchLists = await _watchListRepository.GetAllAsync(includeProperties: "Stocks");

                // Apply search filter
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    watchLists = watchLists.Where(w => w.Label.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
                }

                // Apply sorting
                if (!string.IsNullOrWhiteSpace(sortColumn))
                {
                    if (sortOrder?.ToLower() == "desc")
                    {
                        watchLists = watchLists.OrderByDescending(GetSortProperty(sortColumn));
                    }
                    else
                    {
                        watchLists = watchLists.OrderBy(GetSortProperty(sortColumn));
                    }
                }

                var totalCount = watchLists.Count();

                // Apply pagination
                var paginatedWatchLists = watchLists
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return new PageListResponse<WatchListDTO>
                {
                    Items = MapWatchListsToDTOs(paginatedWatchLists),
                    Page = page,
                    PageSize = pageSize,
                    TotalCount = totalCount,
                    HasNextPage = (page * pageSize) < totalCount,
                    HasPreviousPage = page > 1
                };
            }

            public async Task<WatchListDTO> GetWatchListByIdAsync(int id)
            {
                var watchList = await _watchListRepository.GetAsync(w => w.WatchListId == id, includeProperties: "Stocks")
                                 ?? throw new KeyNotFoundException("Watchlist not found.");
                return MapWatchListToDTO(watchList);
            }
            public async Task<IEnumerable<WatchListDTO>> GetWatchListsByUserIdAsync(int userId, string? searchTerm = null)
            {
                // Lấy tất cả watchlist của user với các quan hệ liên quan
                var watchLists = await _watchListRepository.GetAllAsync(
                    w => w.UserId == userId,
                    includeProperties: "Stocks,Stocks.Company,Stocks.Market"
                );

                // Kiểm tra dữ liệu gốc
                if (watchLists == null || !watchLists.Any())
                {
                    return Enumerable.Empty<WatchListDTO>(); // Trả về rỗng nếu không có dữ liệu
                }

                // Chỉ lọc nếu searchTerm hợp lệ (khác null và không rỗng)
                if (!string.IsNullOrWhiteSpace(searchTerm)) // Dùng IsNullOrWhiteSpace để chắc chắn
                {
                    watchLists = watchLists.Select(w =>
                    {
                        w.Stocks = w.Stocks.Where(s =>
                            s.StockSymbol.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                            (s.Company?.CompanyName != null && s.Company.CompanyName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                        ).ToList();
                        return w;
                    }).Where(w => w.Stocks.Any()).ToList(); // Chỉ giữ lại watchlist có ít nhất 1 stock khớp
                }

                // Chuyển đổi sang DTO và trả về
                return watchLists.Select(MapWatchListToDTO).ToList();
            }
        public async Task<WatchListDTO> CreateWatchListAsync(CreateWatchListDTO dto)
            {
                var watchList = new WatchList
                {
                    UserId = dto.UserId,
                    Label = dto.Label,
                    Status = dto.Status,
                    CreatedDate = DateTime.Now,
                    LastEdited = DateTime.Now
                };

                await _watchListRepository.AddAsync(watchList);
                return MapWatchListToDTO(watchList);
            }

            public async Task<WatchListDTO> UpdateWatchListAsync(int id, UpdateWatchListDTO dto)
            {
                var watchList = await _watchListRepository.GetAsync(w => w.WatchListId == id, includeProperties: "Stocks")
                                 ?? throw new KeyNotFoundException("Watchlist not found.");

                watchList.Label = dto.Label;
                watchList.Status = dto.Status;
                watchList.LastEdited = DateTime.Now;

                var updatedWatchList = await _watchListRepository.UpdateAsync(watchList);
                return MapWatchListToDTO(updatedWatchList);
            }

            public async Task<bool> DeleteWatchListAsync(int id)
            {
                var watchList = await _watchListRepository.GetAsync(w => w.WatchListId == id);
                if (watchList == null) return false;

                await _watchListRepository.DeleteAsync(watchList);
                return true;
            }

            private static Func<WatchList, object> GetSortProperty(string sortColumn)
            {
                return sortColumn?.ToLower() switch
                {
                    "label" => watchList => watchList.Label,
                    "createddate" => watchList => watchList.CreatedDate,
                    "lastedited" => watchList => watchList.LastEdited,
                    _ => watchList => watchList.WatchListId
                };
            }

            private static WatchListDTO MapWatchListToDTO(WatchList watchList)
            {
                var stockDTOs = watchList.Stocks
                    .Select(s => new StockDTO
                    {
                        StockId = s.StockId,
                        CompanyId = s.CompanyId,
                        StockSymbol = s.StockSymbol,
                        MarketId = s.MarketId,
                        ListedDate = s.ListedDate,
                        CompanyName = s.Company?.CompanyName,
                        MarketName = s.Market?.MarketName,
                    })
                    .ToList();
                
                return new WatchListDTO
                {
                    WatchListId = watchList.WatchListId,
                    UserId = watchList.UserId,
                    Label = watchList.Label,
                    CreatedDate = watchList.CreatedDate,
                    LastEdited = watchList.LastEdited,
                    Status = watchList.Status,
                    Stocks = stockDTOs

                };
            }

            private List<WatchListDTO> MapWatchListsToDTOs(List<WatchList> watchLists)
            {
                return watchLists.Select(MapWatchListToDTO).ToList();
            }
        }
    
}
