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
            service.AddTransient<ICompanyRepository, CompanyRepository>();
            service.AddTransient<INewsRepository, NewsRepository>();
            service.AddTransient<IMarketRepository, MarketRepository>();
            service.AddTransient<INotificationRepository, NotificationRepository>();
            service.AddTransient<INotificationUserRepository, NotificationUserRepository>();
            service.AddTransient<ISessionRepository, SessionRepository>();
            service.AddTransient<IStockHistoryRepository, StockHistoryRepository>();
            service.AddTransient<IStockInSessionRepository, StockInSessionRepository>();
            service.AddTransient<IStockRopository, StockRepository>();
            service.AddTransient<IUserRepository, UserRepository>();
            service.AddTransient<IWatchListRepository, WatchListRepository>();

            return service;
        }
    }
}
