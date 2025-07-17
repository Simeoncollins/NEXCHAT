using NEXCHAT.CoreBusiness.Enums;

namespace NEXCHAT.UseCases.NotificationManagement.Interfaces
{
    public interface ISendNotificationUseCase
    {
        Task ExecuteAsync(Guid userId, NotificationType notificationType, string content);
    }
}