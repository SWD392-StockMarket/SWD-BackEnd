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
    public class WatchListRepository : Repository<WatchList> , IWatchListRepository
    {
        private readonly StockMarketDbContext _dbcontext;
        public WatchListRepository(StockMarketDbContext context) : base(context)
        {
            _dbcontext = context;
        }
    }
}
