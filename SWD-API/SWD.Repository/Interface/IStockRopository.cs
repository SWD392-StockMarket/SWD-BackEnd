using SWD.Data.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Repository.Interface
{
    public interface IStockRopository : IRepository<Stock>
    {
        //Task<ICollection<Stock>> GetAllStocksAsync();
        Task<bool> AnyAsync(Expression<Func<Stock, bool>> predicate);

        Task<IEnumerable<Stock>> GetAllAsyncInclude(Expression<Func<Stock, bool>>? filter = null, string? includeProperties = null);
    }
}
