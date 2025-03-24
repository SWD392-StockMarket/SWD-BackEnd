using SWD.Data;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SWD.Repository.Repositories
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        private readonly StockMarketDbContext _dbcontext;
        public SessionRepository(StockMarketDbContext context) : base(context)
        {
            _dbcontext = context;
        }

        public async Task<List<Session>> GetSessionsByStockIdAsync(int stockId)
        {
            return await _dbcontext.Sessions.Where(s => s.StockId == stockId).ToListAsync();
        }
    }
}
