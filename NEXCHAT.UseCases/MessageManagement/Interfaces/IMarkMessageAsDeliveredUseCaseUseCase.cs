namespace NEXCHAT.UseCases.MessageManagement.Interfaces
{
    public interface IMarkMessageAsDeliveredUseCase
    {
        Task ExecuteAsync(Guid messageId);
    }
}