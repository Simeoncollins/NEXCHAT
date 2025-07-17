using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness;
using NEXCHAT.CoreBusiness.Enums;

namespace NEXCHAT.UseCases.PluginInterfaces
{
    public interface INotificationRepository
    {
        Task<int> GetUnseenNotificationCountAsync(Guid userId);
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(Guid userId);
        Task MarkNotificationsAsSeenAsync(List<Guid> notificationIds);
        Task SendNotificationAsync(Guid userId, NotificationType notificationType, string content);
    }
}
