using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NEXCHAT.CoreBusiness.Enums;
using NEXCHAT.CoreBusiness.Hubs;
using NEXCHAT.CoreBusiness;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.Plugin.EFCore
{
    public class NotificationRepositoryEfCore : INotificationRepository
    {
        private readonly IDbContextFactory<NEXCHATDBContext> _dbContextFactory;
        private readonly IHubContext<ChatHub> _hubContext;

        public NotificationRepositoryEfCore(
            IDbContextFactory<NEXCHATDBContext> dbContextFactory,
            IHubContext<ChatHub> hubContext)
        {
            _dbContextFactory = dbContextFactory;
            _hubContext = hubContext;
        }

        public async Task<int> GetUnseenNotificationCountAsync(Guid userId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Notifications
                .Where(n => n.UserId == userId && !n.IsSeen)
                .CountAsync();
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.DateSent)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task MarkNotificationsAsSeenAsync(List<Guid> notificationIds)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var notifications = await context.Notifications
                .Where(n => notificationIds.Contains(n.NotificationId))
                .ToListAsync();

            var now = DateTime.UtcNow;
            foreach (var notification in notifications)
            {
                notification.IsSeen = true;
            }

            await context.SaveChangesAsync();
        }

        public async Task SendNotificationAsync(Guid userId, NotificationType notificationType, string content)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var notification = new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = userId,
                NotificationType = notificationType,
                Content = content,
                DateSent = DateTime.UtcNow,
                IsSeen = false
            };

            await context.Notifications.AddAsync(notification);
            await context.SaveChangesAsync();

            // Send real-time update
            await _hubContext.Clients.Group($"user-{userId}")
                .SendAsync("ReceiveNotification", notification);
        }
    }
}
