using FirebaseAdmin.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SWD.Data.Entities;
using SWD.Repository.Interface;
using SWD.Service.Interface;

namespace SWD.Service.Services
{
    public class NotificationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationBackgroundService> _logger;

        public NotificationBackgroundService(IServiceProvider serviceProvider, ILogger<NotificationBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotificationBackgroundService started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("Checking for pending notifications at {Time}", DateTime.UtcNow);
                using (var scope = _serviceProvider.CreateScope())
                {
                    var notificationRepository = scope.ServiceProvider.GetRequiredService<INotificationRepository>();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    var now = DateTime.UtcNow;
                    var pendingNotifications = await notificationRepository.GetAllAsync(n => 
                        n.Status == "Pending" && n.ScheduledTime <= now);

                    if (!pendingNotifications.Any())
                    {
                        _logger.LogDebug("No pending notifications found.");
                    }

                    foreach (var notification in pendingNotifications)
                    {
                        try
                        {
                            await notificationService.SendNotificationAsync(notification);
                            _logger.LogInformation("Processed notification {Id}", notification.NotificationId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to process notification {Id}", notification.NotificationId);
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
            _logger.LogInformation("NotificationBackgroundService stopped.");
        }
    }
}