namespace NEXCHAT.UseCases.MessageManagement.Interfaces
{
    public interface IDeleteMessageUseCase
    {
        Task ExecuteAsync(Guid messageId);
    }
}