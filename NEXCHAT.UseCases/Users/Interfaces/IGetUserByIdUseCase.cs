using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.Users.Interfaces
{
    public interface IGetUserByIdUseCase
    {
        Task<User> ExecuteAsync(Guid userId);
    }
}