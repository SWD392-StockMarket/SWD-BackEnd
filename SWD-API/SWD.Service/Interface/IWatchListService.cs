﻿

using SWD.Data.DTOs.WatchLists;
using SWD.Data.DTOs;

namespace SWD.Service.Interface
{
    public interface IWatchListService
    {
        Task<PageListResponse<WatchListDTO>> GetWatchListsAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20);

        Task<WatchListDTO> GetWatchListByIdAsync(int id);

        Task<WatchListDTO> CreateWatchListAsync(CreateWatchListDTO dto);

        Task<IEnumerable<WatchListDTO>> GetWatchListsByUserIdAsync(int userId);
        Task<WatchListDTO> UpdateWatchListAsync(int id, UpdateWatchListDTO dto);

        Task<bool> DeleteWatchListAsync(int id);
    }
}
