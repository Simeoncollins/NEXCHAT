using NEXCHAT.CoreBusiness.Enums;

namespace NEXCHAT.UseCases.Users.Interfaces
{
    public interface IUpdateUserStatusUseCase
    {
        Task ExecuteAsync(Guid userId, StatusType statusType);
    }
}