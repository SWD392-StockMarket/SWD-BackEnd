using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SWD.Data.Entities;
using SWD.Service.Interface;
using SWD.Service.Services;


namespace SWD.Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection service)
        {
            service.AddTransient<IAuthService, AuthService>();
            service.AddTransient<IStockService, StockService>();
            service.AddTransient<ICompanyService, CompanyService>();
            service.AddTransient<IMarketService, MarketService>();
            service.AddTransient<INewsService, NewsService>();
            service.AddTransient<INotificationService, NotificationService>();
            service.AddTransient<IUserService, UserService>();
            service.AddTransient<IWatchListService, WatchListService>();
            service.AddTransient<IStockHistoryService, StockHistoryService>();
            service.AddTransient<ISessionService, SessionService>();
            service.AddTransient<INotificationUserService, NotificationUserService>();
            service.AddTransient<IEmailSender<User>, SmtpEmailSender>();
            service.AddTransient<IDeviceTokenService, DeviceTokenService>();
            service.AddTransient<IPythonExecuteService, PythonExecuteService>();
            //service.AddTransient<IRoomService, RoomService>();
            return service;
        }
    }
}
