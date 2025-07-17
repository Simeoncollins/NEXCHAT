using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.ConversationManagement.Interfaces
{
    public interface IGetUserConversationsUseCase
    {
        Task<IEnumerable<Conversation>> ExecuteAsync(Guid userId, bool isGroupConversation = false);
    }
}