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
    public class SendNotificationUseCase : ISendNotificationUseCase
    {
        private readonly INotificationRepository iNotificationRepository;

        public SendNotificationUseCase(INotificationRepository iNotificationRepository)
        {
            this.iNotificationRepository = iNotificationRepository;
        }

        public async Task ExecuteAsync(Guid userId, NotificationType notificationType, string content)
        {
            await iNotificationRepository.SendNotificationAsync(userId, notificationType, content);
        }
    }
}
