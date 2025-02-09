using Microsoft.EntityFrameworkCore;
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
    public class StockRepository : IStockRopository
    {
        private readonly StockMarketDbContext _dbcontext;
        public StockRepository(StockMarketDbContext context)
        {
            _dbcontext = context;
        }
        public async Task<ICollection<Stock>> GetAllStocksAsync()
        {
            return await _dbcontext.Stocks.ToListAsync();
        }
    }
}
