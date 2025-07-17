using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.FriendManagement.Interfaces
{
    public interface IGetBlockedFriendsUseCase
    {
        Task<IEnumerable<User>> ExecuteAsync(Guid userId);
    }
}