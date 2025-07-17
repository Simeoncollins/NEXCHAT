namespace NEXCHAT.UseCases.MessageManagement.Interfaces
{
    public interface ISendMessageUseCase
    {
        Task<Guid> ExecuteAsync(Guid senderId, Guid conversationId, string content);
    }
}