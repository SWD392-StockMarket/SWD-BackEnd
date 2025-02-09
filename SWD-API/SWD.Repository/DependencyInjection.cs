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
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            //service.AddTransient<IRoomService, RoomService>();
            service.AddTransient<IStockRopository, StockRepository>();
            return service;
        }
    }
}
