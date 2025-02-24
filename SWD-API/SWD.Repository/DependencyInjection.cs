using Microsoft.Extensions.DependencyInjection;
using SWD.Repository.Interface;
using SWD.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Repository
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection service)
        {
            service.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            service.AddTransient<IStockRopository, StockRepository>();
            return service;
        }
    }
}
