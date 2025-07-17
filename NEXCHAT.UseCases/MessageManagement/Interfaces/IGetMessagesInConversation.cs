using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.MessageManagement.Interfaces
{
    public interface IGetMessagesInConversationUseCase
    {
        Task<IEnumerable<Message>> ExecuteAsync(Guid conversationId, int page, int pageSize);
    }
}