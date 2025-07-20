using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS;
using NEXCHAT.UseCases.NotificationManagement.Interfaces;

namespace NEXCHAT.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IGetUnseenNotificationCountUseCase _getUnseenNotificationCountUseCase;
        private readonly IGetUserNotificationsUseCase _getUserNotificationsUseCase;
        private readonly IMarkNotificationsAsSeenUseCase _markNotificationsAsSeenUseCase;
        private readonly ISendNotificationUseCase _sendNotificationUseCase;

        public NotificationsController(
            IGetUnseenNotificationCountUseCase getUnseenNotificationCountUseCase,
            IGetUserNotificationsUseCase getUserNotificationsUseCase,
            IMarkNotificationsAsSeenUseCase markNotificationsAsSeenUseCase,
            ISendNotificationUseCase sendNotificationUseCase)
        {
            _getUnseenNotificationCountUseCase = getUnseenNotificationCountUseCase;
            _getUserNotificationsUseCase = getUserNotificationsUseCase;
            _markNotificationsAsSeenUseCase = markNotificationsAsSeenUseCase;
            _sendNotificationUseCase = sendNotificationUseCase;
        }

        // GET: api/notifications/unseen-count/{userId}
        [HttpGet("unseen-count/{userId}")]
        public async Task<IActionResult> GetUnseenCount(Guid userId)
        {
            var count = await _getUnseenNotificationCountUseCase.ExecuteAsync(userId);
            return Ok(count);
        }

        // GET: api/notifications/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserNotifications(Guid userId)
        {
            var notifications = await _getUserNotificationsUseCase.ExecuteAsync(userId);
            return Ok(notifications);
        }

        // POST: api/notifications/mark-as-seen
        [HttpPost("mark-as-seen")]
        public async Task<IActionResult> MarkNotificationsAsSeen([FromBody] List<Guid> notificationIds)
        {
            if (notificationIds == null || !notificationIds.Any())
                return BadRequest("No notification IDs provided.");

            await _markNotificationsAsSeenUseCase.ExecuteAsync(notificationIds);
            return NoContent(); // 204
        }

        // POST: api/notifications/send
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] SendNotificationDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid notification data.");

            await _sendNotificationUseCase.ExecuteAsync(dto.UserId, dto.Type, dto.Content);
            return NoContent(); // 204
        }
    }

   
}
