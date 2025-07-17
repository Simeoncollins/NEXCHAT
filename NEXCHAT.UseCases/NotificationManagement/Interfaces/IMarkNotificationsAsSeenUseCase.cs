namespace NEXCHAT.UseCases.NotificationManagement.Interfaces
{
    public interface IMarkNotificationsAsSeenUseCase
    {
        Task ExecuteAsync(List<Guid> notificationIds);
    }
}