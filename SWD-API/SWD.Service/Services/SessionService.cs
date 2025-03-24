using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SWD.Data.DTOs;
using SWD.Data.DTOs.Session;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Service.Interface;


namespace SWD.Service.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionService(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }

        public async Task<PageListResponse<SessionDTO>> GetSessionsAsync(string? searchTerm, string? sortColumn, string? sortOrder, int page = 1, int pageSize = 20)
        {
            var sessions = await _sessionRepository.GetAllAsync();

            // Apply search filter
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                sessions = sessions.Where(s => s.SessionType.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }

            // Apply sorting
            if (!string.IsNullOrWhiteSpace(sortColumn))
            {
                if (sortOrder?.ToLower() == "desc")
                {
                    sessions = sessions.OrderByDescending(GetSortProperty(sortColumn));
                }
                else
                {
                    sessions = sessions.OrderBy(GetSortProperty(sortColumn)).ToList();
                }
            }

            var totalCount = sessions.Count();

            // Apply pagination
            var paginatedSessions = sessions
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PageListResponse<SessionDTO>
            {
                Items = MapSessionsToDTOs(paginatedSessions),
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                HasNextPage = (page * pageSize) < totalCount,
                HasPreviousPage = page > 1
            };
        }

        public async Task<SessionDTO> GetSessionByIdAsync(int id)
        {
            var session = await _sessionRepository.GetAsync(s => s.SessionId == id)
                          ?? throw new KeyNotFoundException("Session not found.");

            return MapSessionToDTO(session);
        }

        public async Task<SessionDTO> CreateSessionAsync(CreateSessionDTO dto)
        {
            var session = new Session
            {
                SessionType = dto.SessionType,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = dto.Status
            };

            await _sessionRepository.AddAsync(session);
            return MapSessionToDTO(session);
        }

        public async Task<SessionDTO> UpdateSessionAsync(int id, UpdateSessionDTO dto)
        {
            var session = await _sessionRepository.GetAsync(s => s.SessionId == id)
                          ?? throw new KeyNotFoundException("Session not found.");

            session.SessionType = dto.SessionType;
            session.StartTime = dto.StartTime;
            session.EndTime = dto.EndTime;
            session.Status = dto.Status;

            var updatedSession = await _sessionRepository.UpdateAsync(session);
            return MapSessionToDTO(updatedSession);
        }

        public async Task<bool> DeleteSessionAsync(int id)
        {
            var session = await _sessionRepository.GetAsync(s => s.SessionId == id);
            if (session == null) return false;

            await _sessionRepository.DeleteAsync(session);
            return true;
        }

        public async Task<List<StockInSession>> GetSessionStocksAsync(int sessionId)
        {
            var session = await _sessionRepository.GetAsync(s => s.SessionId == sessionId, includeProperties: "StockInSessions");
            if (session == null)
            {
                throw new KeyNotFoundException("Session not found.");
            }

            return session.StockInSessions.ToList();
        }

        public async Task<List<SessionDTO>> GetSessionsAsyncByStockId(int stockId)
        {
            var list = await _sessionRepository.GetSessionsByStockIdAsync(stockId);
            return MapSessionsToDTOs(list);
        }

        private static Func<Session, object> GetSortProperty(string sortColumn)
        {
            return sortColumn?.ToLower() switch
            {
                "sessiontype" => session => session.SessionType,
                "starttime" => session => session.StartTime,
                "endtime" => session => session.EndTime,
                "status" => session => session.Status,
                _ => session => session.SessionId
            };
        }

        private static SessionDTO MapSessionToDTO(Session session)
        {
            return new SessionDTO
            {
                SessionId = session.SessionId,
                SessionType = session.SessionType,
                StartTime = session.StartTime,
                EndTime = session.EndTime,
                Status = session.Status
            };
        }

        private List<SessionDTO> MapSessionsToDTOs(List<Session> sessions)
        {
            return sessions.Select(MapSessionToDTO).ToList();
        }
    }
}
