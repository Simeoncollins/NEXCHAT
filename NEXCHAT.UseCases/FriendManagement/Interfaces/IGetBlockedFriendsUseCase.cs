using NEXCHAT.CoreBusiness;
using NEXCHAT.CoreBusiness.Classes;

namespace NEXCHAT.UseCases.FriendManagement.Interfaces
{
    public interface IGetBlockedFriendsUseCase
    {
        Task<IEnumerable<FriendDto>> ExecuteAsync(Guid userId);
    }
}