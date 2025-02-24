using Microsoft.EntityFrameworkCore;
using SWD.Data;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Repository.Repositories
{
    public class StockRepository : Repository<Stock>,IStockRopository
    {
        private readonly StockMarketDbContext _dbcontext;
        public StockRepository(StockMarketDbContext context) :base(context)
        {
            _dbcontext = context;
        }

        public async Task<bool> AnyAsync(Expression<Func<Stock, bool>> predicate)
        {
            return await _dbcontext.Set<Stock>().AnyAsync(predicate);
        }

        
    }
}
