namespace NEXCHAT.UseCases.ConversationManagement.Interfaces
{
    public interface IRemoveParticipantFromConversationUseCase
    {
        Task ExecuteAsync(Guid conversationId, Guid userId);
    }
}