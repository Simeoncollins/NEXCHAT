namespace NEXCHAT.UseCases.ConversationManagement.Interfaces
{
    public interface IAddParticipantToConversationUseCase
    {
        Task ExecuteAsync(Guid conversationId, Guid userId);
    }
}