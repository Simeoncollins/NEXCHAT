namespace NEXCHAT.UseCases.MessageManagement.Interfaces
{
    public interface IRemoveReactionFromMessageUseCase
    {
        Task ExecuteAsync(Guid messageId, Guid userId);
    }
}