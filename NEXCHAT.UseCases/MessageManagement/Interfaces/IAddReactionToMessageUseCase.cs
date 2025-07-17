namespace NEXCHAT.UseCases.MessageManagement.Interfaces
{
    public interface IAddReactionToMessageUseCase
    {
        Task ExecuteAsync(Guid messageId, Guid userId, Guid reactionId);
    }
}