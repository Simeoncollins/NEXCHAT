using NEXCHAT.CoreBusiness;
using NEXCHAT.CoreBusiness.Classes;

namespace NEXCHAT.UseCases.FriendManagement.Interfaces
{
    public interface IGetFriendListUseCase
    {
        Task<IEnumerable<FriendDto>> ExecuteAsync(Guid userId);
    }
}