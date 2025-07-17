using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness;
using NEXCHAT.CoreBusiness.Enums;

namespace NEXCHAT.UseCases.PluginInterfaces
{
    public interface IUserRepository
    {
        Task AcceptFriendRequestAsync(Guid friendRequestId);
        Task BlockFriendAsync(Guid userId, Guid friendId);
        Task<IEnumerable<User>> GetBlockedFriendsAsync(Guid userId);
        Task<IEnumerable<User>> GetFriendListAsync(Guid userId);
        Task<IEnumerable<User>> GetPendingFriendRequestsAsync(Guid userId);
        Task<User> GetUserByIdAsync(Guid userId);
        Task<IEnumerable<User>> GetUsersByNameAsync(string name, int pageIndex, int pageSize);
        Task RejectFriendRequestAsync(Guid friendRequestId);
        Task SendFriendRequestAsync(Guid requesterId, Guid receiverId);
        Task UnBlockFriendAsync(Guid userId, Guid friendId);
        Task UpdateStatusAsync(Guid userId, StatusType statusType);
    }
}
