namespace NEXCHAT.UseCases.MessageManagement.Interfaces
{
    public interface IEditMessageUseCase
    {
        Task ExecuteAsync(Guid messageId, string newContent);
    }
}