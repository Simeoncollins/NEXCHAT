using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.MessageManagement.Interfaces
{
    public interface ISendMessageUseCase
    {
        Task<Message> ExecuteAsync(Message message);
    }
}