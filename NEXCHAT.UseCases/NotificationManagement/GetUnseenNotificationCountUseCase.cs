using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.NotificationManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.NotificationManagement
{
    public class GetUnseenNotificationCountUseCase : IGetUnseenNotificationCountUseCase
    {
        private readonly INotificationRepository iNotificationRepository;

        public GetUnseenNotificationCountUseCase(INotificationRepository iNotificationRepository)
        {
            this.iNotificationRepository = iNotificationRepository;
        }

        public async Task<int> ExecuteAsync(Guid userId)
        {
            return await iNotificationRepository.GetUnseenNotificationCountAsync(userId);
        }
    }
}
