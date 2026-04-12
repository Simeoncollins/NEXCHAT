using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.ConversationManagement.Interfaces
{
    public interface IStartConversationUseCase
    {
        Task<Guid> ExecuteAsync(Guid creatorId, Guid conversationId, List<Guid> initialParticipantsId, bool isGroup, Message message, string groupName = "");
    }
}