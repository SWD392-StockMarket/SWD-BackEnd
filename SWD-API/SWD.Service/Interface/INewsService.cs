using SWD.Data.DTOs.News;
using SWD.Data.DTOs.User;
using SWD.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Service.Interface
{
    public interface INewsService
    {
        Task<PageListResponse<NewsDTO>> GetNewsAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20);
        Task<NewsDTO> GetNewsByIdAsync(int id);
        Task<NewsDTO> CreateNewsAsync(CreateNewsDTO dto);
        Task<NewsDTO> UpdateNewsAsync(int id, UpdateNewsDTO dto);
        Task<bool> DeleteNewsAsync(int id);
        Task<List<NewsDTO>> GetNewsByTypeAsync(string type);
        Task<List<NewsDTO>> GetNewsByStatusAsync(string status);
        Task<NewsAuthor> GetNewsAuthorAsync(int id);
    }
}
