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

        public async Task<IEnumerable<Stock>> GetAllAsyncInclude(Expression<Func<Stock, bool>>? filter = null, string? includeProperties = null)
        {
            IQueryable<Stock> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter).Include(s => s.Company).Include(s => s.Market);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp.Trim());
                }
            }

            return await query.ToListAsync();        }
    }
}
