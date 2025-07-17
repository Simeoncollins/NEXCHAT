using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.FriendManagement.Interfaces
{
    public interface IGetPendingFriendRequestsUseCase
    {
        Task<IEnumerable<User>> ExecuteAsync(Guid userId);
    }
}