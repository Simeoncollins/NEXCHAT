using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness;
using NEXCHAT.CoreBusiness.Classes;
using NEXCHAT.CoreBusiness.Enums;

namespace NEXCHAT.UseCases.PluginInterfaces
{
    public interface IUserRepository
    {
        Task AcceptFriendRequestAsync(Guid requesterId, Guid receiverId);
        Task BlockFriendAsync(Guid userId, Guid friendId);
        Task<IEnumerable<FriendDto>> GetBlockedFriendsAsync(Guid userId);
        Task<IEnumerable<FriendDto>> GetFriendListAsync(Guid userId);
        Task<IEnumerable<FriendDto>> GetPendingFriendRequestsAsync(Guid userId);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetUsersByNameAsync(string name, int pageIndex, int pageSize);
        Task RejectFriendRequestAsync(Guid requesterId, Guid receiverId);
        Task SendFriendRequestAsync(Guid requesterId, Guid receiverId);
        Task UnBlockFriendAsync(Guid userId, Guid friendId);
        Task UpdateStatusAsync(Guid userId, StatusType statusType);
    }
}
