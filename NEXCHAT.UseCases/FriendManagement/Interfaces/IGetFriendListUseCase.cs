using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.FriendManagement.Interfaces
{
    public interface IGetFriendListUseCase
    {
        Task<IEnumerable<User>> ExecuteAsync(Guid userId);
    }
}