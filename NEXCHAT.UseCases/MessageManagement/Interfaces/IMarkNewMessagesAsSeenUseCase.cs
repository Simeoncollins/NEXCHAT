namespace NEXCHAT.UseCases.MessageManagement.Interfaces
{
    public interface IMarkNewMessagesAsSeenUseCase
    {
        Task ExecuteAsync(Guid conversationId, Guid userId);
    }
}