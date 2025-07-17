namespace NEXCHAT.UseCases.NotificationManagement.Interfaces
{
    public interface IGetUnseenNotificationCountUseCase
    {
        Task<int> ExecuteAsync(Guid userId);
    }
}