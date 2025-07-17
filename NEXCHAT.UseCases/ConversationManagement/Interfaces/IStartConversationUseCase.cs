namespace NEXCHAT.UseCases.ConversationManagement.Interfaces
{
    public interface IStartConversationUseCase
    {
        Task<Guid> ExecuteAsync(Guid creatorId, List<Guid> initialParticipantsId, bool isGroup, string groupName = "");
    }
}