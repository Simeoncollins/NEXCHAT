using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NEXCHAT.CoreBusiness;
using NEXCHAT.UseCases.FriendManagement.Interfaces;

namespace NEXCHAT.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : ControllerBase
    {
        private readonly IAcceptFriendRequestUseCase acceptFriendRequestUseCase;
        private readonly IBlockFriendUseCase blockFriendUseCase;
        private readonly IGetBlockedFriendsUseCase getBlockedFriendsUseCase;
        private readonly IGetFriendListUseCase getFriendListUseCase;
        private readonly IGetPendingFriendRequestsUseCase getPendingFriendRequestsUseCase;
        private readonly IRejectFriendRequestUseCase rejectFriendRequestUseCase;
        private readonly ISendFriendRequestUseCase sendFriendRequestUseCase;
        private readonly IUnBlockFriendUseCase unBlockFriendUseCase;

        public FriendsController
            (
            IAcceptFriendRequestUseCase acceptFriendRequestUseCase,
            IBlockFriendUseCase blockFriendUseCase,
            IGetBlockedFriendsUseCase getBlockedFriendsUseCase,
            IGetFriendListUseCase getFriendListUseCase,
            IGetPendingFriendRequestsUseCase getPendingFriendRequestsUseCase,
            IRejectFriendRequestUseCase rejectFriendRequestUseCase,
            ISendFriendRequestUseCase sendFriendRequestUseCase,
            IUnBlockFriendUseCase unBlockFriendUseCase
            )
        {
            this.acceptFriendRequestUseCase = acceptFriendRequestUseCase;
            this.blockFriendUseCase = blockFriendUseCase;
            this.getBlockedFriendsUseCase = getBlockedFriendsUseCase;
            this.getFriendListUseCase = getFriendListUseCase;
            this.getPendingFriendRequestsUseCase = getPendingFriendRequestsUseCase;
            this.rejectFriendRequestUseCase = rejectFriendRequestUseCase;
            this.sendFriendRequestUseCase = sendFriendRequestUseCase;
            this.unBlockFriendUseCase = unBlockFriendUseCase;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<User>>> GetFriendList(Guid userId)
        {
            var friends = await getFriendListUseCase.ExecuteAsync(userId);
            return Ok(friends);
        }

        [HttpGet("{userId}/requests")]
        public async Task<ActionResult<IEnumerable<User>>> GetPendingRequests(Guid userId)
        {
            var requests = await getPendingFriendRequestsUseCase.ExecuteAsync(userId);
            return Ok(requests);
        }

        [HttpGet("{userId}/blocked")]
        public async Task<ActionResult<IEnumerable<User>>> GetBlockedFriends(Guid userId)
        {
            var bocked = await getBlockedFriendsUseCase.ExecuteAsync(userId);
            return Ok(bocked);
        }

        [HttpPost("{requesterId}/send")]
        public async Task<IActionResult> SendFriendRequest(Guid requesterId, [FromQuery] Guid recieverId)
        {
            await sendFriendRequestUseCase.ExecuteAsync(requesterId, recieverId);
            return NoContent();
        }
        
        [HttpPost("{requestId}/accept")]
        public async Task<IActionResult> AcceptFriendRequest(Guid requestId)
        {
            await acceptFriendRequestUseCase.ExecuteAsync(requestId);
            return NoContent();
        }

        [HttpPost("{requestId}/reject")]
        public async Task<IActionResult> RejectFriendRequest(Guid requestId)
        {
            await rejectFriendRequestUseCase.ExecuteAsync(requestId);
            return NoContent();
        }

        [HttpPost("{userId}/block")]
        public async Task<IActionResult> BlockFriend(Guid userId, [FromQuery] Guid friendId)
        {
            await blockFriendUseCase.ExecuteAsync(userId, friendId);
            return NoContent();
        }

        [HttpPost("{userId}/unblock")]
        public async Task<IActionResult> UnblockFriend(Guid userId, [FromQuery] Guid friendId)
        {
            await unBlockFriendUseCase.ExecuteAsync(userId, friendId);
            return NoContent();
        }

    }
}
