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
    public class NewsRepository : Repository<News>, INewsRepository
    {
        private readonly StockMarketDbContext _dbcontext;
        public NewsRepository(StockMarketDbContext context) : base(context)
        {
            _dbcontext = context;
            
        }
    }
}
