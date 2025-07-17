using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NEXCHAT.CoreBusiness;
using NEXCHAT.Server.DTOS;
using NEXCHAT.UseCases.ConversationManagement.Interfaces;

namespace NEXCHAT.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationsController : ControllerBase
    {
        private readonly IAddParticipantToConversationUseCase addParticipantToConversationUseCase;
        private readonly IGetConversationUseCase getConversationUseCase;
        private readonly IGetUnreadConversationCountUseCase getUnreadConversationCountUseCase;
        private readonly IGetUserConversationsUseCase getUserConversationsUseCase;
        private readonly IRemoveParticipantFromConversationUseCase removeParticipantFromConversationUseCase;
        private readonly ISetTypingIndicatorUseCase setTypingIndicatorUseCase;
        private readonly IStartConversationUseCase startConversationUseCase;
        private readonly IUpdateGroupDetailsUseCase updateGroupDetailsUseCase;

        public ConversationsController
            (
            IAddParticipantToConversationUseCase addParticipantToConversationUseCase,
            IGetConversationUseCase getConversationUseCase,
            IGetUnreadConversationCountUseCase getUnreadConversationCountUseCase,
            IGetUserConversationsUseCase getUserConversationsUseCase,
            IRemoveParticipantFromConversationUseCase removeParticipantFromConversationUseCase,
            ISetTypingIndicatorUseCase setTypingIndicatorUseCase,
            IStartConversationUseCase startConversationUseCase,
            IUpdateGroupDetailsUseCase updateGroupDetailsUseCase
            )
        {
            this.addParticipantToConversationUseCase = addParticipantToConversationUseCase;
            this.getConversationUseCase = getConversationUseCase;
            this.getUnreadConversationCountUseCase = getUnreadConversationCountUseCase;
            this.getUserConversationsUseCase = getUserConversationsUseCase;
            this.removeParticipantFromConversationUseCase = removeParticipantFromConversationUseCase;
            this.setTypingIndicatorUseCase = setTypingIndicatorUseCase;
            this.startConversationUseCase = startConversationUseCase;
            this.updateGroupDetailsUseCase = updateGroupDetailsUseCase;
        }

        [HttpPost("start")]
        public async Task<ActionResult<Guid>> StartConversation([FromBody] StartConversationRequestDto request)
        {
            var conversationId = await startConversationUseCase.ExecuteAsync(
                request.CreatorId,
                request.Participants,
                request.IsGroup,
                request.GroupName
            );

            return Ok(conversationId);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Conversation>> GetConversation(Guid id)
        {
            var conversation = await getConversationUseCase.ExecuteAsync(id);
            if (conversation == null)
                return NotFound();

            return Ok(conversation);
        }

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Conversation>>> GetUserConversations(Guid userId, [FromQuery] bool isGroup = false)
        {
            var conversations = await getUserConversationsUseCase.ExecuteAsync(userId, isGroup);
            return Ok(conversations);
        }

        [HttpGet("unread-count")]
        public async Task<ActionResult<int>> GetUnreadCount([FromQuery] Guid userId, [FromQuery] bool isGroup = false)
        {
            var count = await getUnreadConversationCountUseCase.ExecuteAsync(userId, isGroup);
            return Ok(count);
        }

        [HttpPost("{conversationId}/add-participant")]
        public async Task<IActionResult> AddParticipant(Guid conversationId, [FromQuery] Guid userId)
        {
            await addParticipantToConversationUseCase.ExecuteAsync(conversationId, userId);
            return NoContent();
        }

        [HttpDelete("{conversationId}/remove-participant")]
        public async Task<IActionResult> RemoveParticipant(Guid conversationId, [FromQuery] Guid userId)
        {
            await removeParticipantFromConversationUseCase.ExecuteAsync(conversationId, userId);
            return NoContent();
        }

        [HttpPost("{conversationId}/typing-indicator")]
        public async Task<IActionResult> SetTypingIndicator(Guid conversationId, [FromQuery] Guid userId, [FromQuery] bool isTyping)
        {
            await setTypingIndicatorUseCase.ExecuteAsync(conversationId, userId, isTyping);
            return NoContent();
        }

        [HttpPut("{conversationId}/group-details")]
        public async Task<IActionResult> UpdateGroupDetails(Guid conversationId, [FromBody] UpdateGroupDetailsRequestDto request)
        {
            await updateGroupDetailsUseCase.ExecuteAsync(conversationId, request.GroupName, request.GroupCoverPhotoPath);
            return NoContent();
        }
    }
}
