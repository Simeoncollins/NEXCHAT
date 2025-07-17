using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness;
using NEXCHAT.CoreBusiness.Enums;
using NEXCHAT.UseCases.NotificationManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.NotificationManagement
{
    public class GetUserNotificationsUseCase : IGetUserNotificationsUseCase
    {
        private readonly INotificationRepository iNotificationRepository;

        public GetUserNotificationsUseCase(INotificationRepository iNotificationRepository)
        {
            this.iNotificationRepository = iNotificationRepository;
        }

        public async Task<IEnumerable<Notification>> ExecuteAsync(Guid userId)
        {
            return await iNotificationRepository.GetUserNotificationsAsync(userId);
        }
    }
}
