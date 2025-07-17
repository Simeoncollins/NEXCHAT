using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness.Enums;
using NEXCHAT.UseCases.NotificationManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.NotificationManagement
{
    public class MarkNotificationsAsSeenUseCase : IMarkNotificationsAsSeenUseCase
    {
        private readonly INotificationRepository iNotificationRepository;

        public MarkNotificationsAsSeenUseCase(INotificationRepository iNotificationRepository)
        {
            this.iNotificationRepository = iNotificationRepository;
        }

        public async Task ExecuteAsync(List<Guid> notificationIds)
        {
            await iNotificationRepository.MarkNotificationsAsSeenAsync(notificationIds);
        }
    }
}
