using SWD.Data;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Repository.Repositories
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {
        private readonly StockMarketDbContext _dbcontext;
        public SessionRepository(StockMarketDbContext context) : base(context)
        {
            _dbcontext = context;
        }
    }
}
