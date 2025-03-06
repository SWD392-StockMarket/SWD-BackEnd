using SWD.Data.DTOs;
using SWD.Data.DTOs.WatchLists;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Service.Interface;

namespace SWD.Service.Services
{
    public class WatchListService : IWatchListService
    {
        
            private readonly IWatchListRepository _watchListRepository;

            public WatchListService(IWatchListRepository watchListRepository)
            {
                _watchListRepository = watchListRepository;
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
            public async Task<IEnumerable<WatchListDTO>> GetWatchListsByUserIdAsync(int userId)
            {
                var watchLists = await _watchListRepository.GetAllAsync(w => w.UserId == userId, includeProperties:"Stocks");

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
                return new WatchListDTO
                {
                    WatchListId = watchList.WatchListId,
                    UserId = watchList.UserId,
                    Label = watchList.Label,
                    CreatedDate = watchList.CreatedDate,
                    LastEdited = watchList.LastEdited,
                    Status = watchList.Status
                };
            }

            private List<WatchListDTO> MapWatchListsToDTOs(List<WatchList> watchLists)
            {
                return watchLists.Select(MapWatchListToDTO).ToList();
            }
        }
    
}
