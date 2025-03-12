using SWD.Data.DTOs;
using SWD.Data.DTOs.News;
using SWD.Data.DTOs.User;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Service.Interface;


namespace SWD.Service.Services
{
    public class NewsService : INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsService(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        public async Task<PageListResponse<NewsDTO>> GetNewsAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20)
        {
            var newsList = await _newsRepository.GetAllAsync();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                newsList = newsList.Where(n =>
                    (n.Title != null && n.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)) ||
                    (n.Content != null && n.Content.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                if (sortOrder?.ToLower() == "desc")
                {
                    newsList = newsList.OrderByDescending(GetSortProperty(sortColumn));
                }
                else
                {
                    newsList = newsList.OrderBy(GetSortProperty(sortColumn));
                }
            }

            var totalCount = newsList.Count();

            // Apply pagination
            var paginatedNews = newsList
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PageListResponse<NewsDTO>
            {
                Items = MapNewsToDTOs(paginatedNews),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                HasNextPage = (page * pageSize) < totalCount,
                HasPreviousPage = page > 1
            };
        }

        public async Task<NewsDTO> GetNewsByIdAsync(int id)
        {
            var news = await _newsRepository.GetAsync(n => n.NewsId == id)
                        ?? throw new KeyNotFoundException("News not found.");

            return MapNewsToDTO(news);
        }

        public async Task<NewsDTO> CreateNewsAsync(CreateNewsDTO dto)
        {
            var news = new News
            {
                StaffId = dto.StaffId,
                Title = dto.Title,
                Content = dto.Content,
                Type = dto.Type,
                CreatedDate = DateTime.UtcNow.AddHours(7),
                Status = dto.Status,
                Url = dto.Url
            };

            await _newsRepository.AddAsync(news);
            return MapNewsToDTO(news);
        }

        public async Task<NewsDTO> UpdateNewsAsync(int id, UpdateNewsDTO dto)
        {
            var news = await _newsRepository.GetAsync(n => n.NewsId == id)
                        ?? throw new KeyNotFoundException("News not found.");
            news.Title = dto.Title;
            news.Content = dto.Content;
            news.Type = dto.Type;
            news.LastEdited = DateTime.UtcNow.AddHours(7);
            news.Url = dto.Url;

            var updatedNews = await _newsRepository.UpdateAsync(news);
            return MapNewsToDTO(updatedNews);
        }

        public async Task<bool> DeleteNewsAsync(int id)
        {
            var news = await _newsRepository.GetAsync(n => n.NewsId == id);
            if (news == null) return false;
        
            news.Status = "Deleted";
            await _newsRepository.UpdateAsync(news);
            return true;
        }
        

        public async Task<List<NewsDTO>> GetNewsByTypeAsync(string type)
        {
            var newsList = (await _newsRepository.GetAllAsync(n => n.Type == type)).ToList();
            return MapNewsToDTOs(newsList);
        }

        public async Task<List<NewsDTO>> GetNewsByStatusAsync(string status)
        {
            var newsList = (await _newsRepository.GetAllAsync(n => n.Status == status)).ToList();
            return MapNewsToDTOs(newsList);
        }

        public async Task<NewsAuthor> GetNewsAuthorAsync(int id)
        {
            var news = await _newsRepository.GetAsync(n => n.NewsId == id, includeProperties: "Staff");
            if (news?.Staff == null) throw new KeyNotFoundException("Author not found.");

            return new NewsAuthor
            {
                StaffId = news.Staff.Id,
                StaffName = news.Staff.UserName,
                Email = news.Staff.Email
            };
        }

        private static Func<News, object> GetSortProperty(string sortColumn)
        {
            return sortColumn?.ToLower() switch
            {
                "title" => news => news.Title,
                "content" => news => news.Content,
                "createddate" => news => news.CreatedDate,
                "type" => news => news.Type,
                _ => news => news.NewsId
            };
        }

        private static NewsDTO MapNewsToDTO(News news)
        {
            return new NewsDTO
            {
                NewsId = news.NewsId,
                StaffId = news.StaffId,
                Title = news.Title,
                Content = news.Content,
                Type = news.Type,
                CreatedDate = news.CreatedDate,
                LastEdited = news.LastEdited,
                Status = news.Status,
                Url = news.Url
            };
        }

        private List<NewsDTO> MapNewsToDTOs(List<News> newsList)
        {
            return newsList.Select(MapNewsToDTO).ToList();
        }
    }
}
