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
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly StockMarketDbContext _dbcontext;
        public UserRepository(StockMarketDbContext context) : base(context)
        {
            _dbcontext = context;
        }
    }
}
