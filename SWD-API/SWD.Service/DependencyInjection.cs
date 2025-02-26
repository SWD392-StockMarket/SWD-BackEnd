using Microsoft.Extensions.DependencyInjection;
using SWD.Service.Interface;
using SWD.Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddTransient<IAuthService, AuthService>();
            service.AddTransient<IStockService, StockService>();
            service.AddTransient<ICompanyService, CompanyService>();
            //service.AddTransient<IRoomService, RoomService>();
            return service;
        }
    }
}
