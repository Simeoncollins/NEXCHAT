using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NEXCHAT.CoreBusiness;
using NEXCHAT.CoreBusiness.Classes;
using NEXCHAT.UseCases.FriendManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;
using NEXCHAT.UseCases.Users;
using NEXCHAT.UseCases.Users.Interfaces;

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
        private readonly IGetUsersByNameUseCase getUsersByNameUseCase;
        private readonly IGetUserByIdUseCase getUserByIdUseCase;
        private readonly ICheckIfBlockedByFriendUseCase checkIfBlockedByFriendUseCase;

        public FriendsController
            (
            IAcceptFriendRequestUseCase acceptFriendRequestUseCase,
            IBlockFriendUseCase blockFriendUseCase,
            IGetBlockedFriendsUseCase getBlockedFriendsUseCase,
            IGetFriendListUseCase getFriendListUseCase,
            IGetPendingFriendRequestsUseCase getPendingFriendRequestsUseCase,
            IRejectFriendRequestUseCase rejectFriendRequestUseCase,
            ISendFriendRequestUseCase sendFriendRequestUseCase,
            IUnBlockFriendUseCase unBlockFriendUseCase,
            IGetUsersByNameUseCase getUsersByNameUseCase,
            IGetUserByIdUseCase getUserByIdUseCase,
            ICheckIfBlockedByFriendUseCase checkIfBlockedByFriendUseCase
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
            this.getUsersByNameUseCase = getUsersByNameUseCase;
            this.getUserByIdUseCase = getUserByIdUseCase;
            this.checkIfBlockedByFriendUseCase = checkIfBlockedByFriendUseCase;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<FriendDto>>> GetFriendList(Guid userId)
        {
            var friends = await getFriendListUseCase.ExecuteAsync(userId);
            return Ok(friends);
        }

        [HttpGet("{userId}/strangers")]
        public async Task<ActionResult<IEnumerable<FriendDto>>> GetPeopleList(Guid userId)
        {
            try
            {
                // Get all users except the current user
                var allUsers = await getUsersByNameUseCase.ExecuteAsync();
                var otherUsers = allUsers.Where(u => u.UserId != userId).ToList();

                // Get the current user with friend request relationships
                var currentUser = await getUserByIdUseCase.ExecuteAsync(userId);

                // Identify strangers and set the StrangerRequested flag
                var strangers = new List<FriendDto>();

                foreach (var user in otherUsers)
                {
                    if (currentUser.isStranger(user.UserId))
                    {
                        var tempUser = new FriendDto
                        (
                            user.UserId,
                            user.UserName,
                            user.Email,
                            user.Status,
                            user.PhotoPath,
                            currentUser.StrangerRequested,
                            false
                        );
                        strangers.Add(tempUser);
                    }
                }

                return Ok(strangers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while processing your request");
            }
        }

        [HttpGet("{userId}/requests")]
        public async Task<ActionResult<IEnumerable<FriendDto>>> GetPendingRequests(Guid userId)
        {
            var requests = await getPendingFriendRequestsUseCase.ExecuteAsync(userId);
            return Ok(requests);
        }

        [HttpGet("{userId}/blocked")]
        public async Task<ActionResult<IEnumerable<FriendDto>>> GetBlockedFriends(Guid userId)
        {
            var blocked = await getBlockedFriendsUseCase.ExecuteAsync(userId);
            return Ok(blocked);
        }

        [HttpPost("{userId}/isBlocked")]
        public async Task<ActionResult<bool>> IsFriendBlocked(Guid userId, [FromQuery] Guid friendId)
        {
            bool isBlocked = await checkIfBlockedByFriendUseCase.ExecuteAsync(userId, friendId);
            return Ok(isBlocked);
        }

        [HttpPost("{requesterId}/send")]
        public async Task<IActionResult> SendFriendRequest(Guid requesterId, [FromQuery] Guid recieverId)
        {
            await sendFriendRequestUseCase.ExecuteAsync(requesterId, recieverId);
            return NoContent();
        }
        
        [HttpPost("{requesterId}/accept")]
        public async Task<IActionResult> AcceptFriendRequest(Guid requesterId, [FromQuery] Guid recieverId)
        {
            await acceptFriendRequestUseCase.ExecuteAsync(requesterId, recieverId);
            return NoContent();
        }
        
        [HttpPost("{requesterId}/reject")]
        public async Task<IActionResult> RejectFriendRequest(Guid requesterId, [FromQuery] Guid recieverId)
        {
            await rejectFriendRequestUseCase.ExecuteAsync(requesterId, recieverId);
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
