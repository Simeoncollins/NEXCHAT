using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.ConversationManagement.Interfaces
{
    public interface IGetConversationUseCase
    {
        Task<Conversation?> ExecuteAsync(Guid conversationId);
    }
}