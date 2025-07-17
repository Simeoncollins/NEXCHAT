namespace NEXCHAT.UseCases.ConversationManagement.Interfaces
{
    public interface IGetUnreadConversationCountUseCase
    {
        Task<int> ExecuteAsync(Guid userId, bool isGroup);
    }
}