using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.NotificationManagement.Interfaces
{
    public interface IGetUserNotificationsUseCase
    {
        Task<IEnumerable<Notification>> ExecuteAsync(Guid userId);
    }
}