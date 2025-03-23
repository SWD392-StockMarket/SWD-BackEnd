
using System.Collections.Generic;
using System.Threading.Tasks;
using SWD.Data.DTOs.Session;
using SWD.Data.DTOs;
using SWD.Data.Entities;

namespace SWD.Service.Interface
{
    public interface ISessionService
    {
        Task<PageListResponse<SessionDTO>> GetSessionsAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20);
        Task<SessionDTO> GetSessionByIdAsync(int id);
        Task<SessionDTO> CreateSessionAsync(CreateSessionDTO dto);
        Task<SessionDTO> UpdateSessionAsync(int id, UpdateSessionDTO dto);
        Task<bool> DeleteSessionAsync(int id);
        Task<List<StockInSession>> GetSessionStocksAsync(int sessionId);
    }
}
