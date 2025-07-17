namespace NEXCHAT.UseCases.ConversationManagement.Interfaces
{
    public interface ISetTypingIndicatorUseCase
    {
        Task ExecuteAsync(Guid conversationId, Guid userId, bool isTyping);
    }
}