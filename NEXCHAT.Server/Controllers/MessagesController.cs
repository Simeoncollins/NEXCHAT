using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NEXCHAT.Server.DTOS;
using NEXCHAT.UseCases.MessageManagement.Interfaces;

namespace NEXCHAT.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IAddReactionToMessageUseCase addReactionToMessageUseCase;
        private readonly IDeleteMessageUseCase deleteMessageUseCase;
        private readonly IEditMessageUseCase editMessageUseCase;
        private readonly IGetMessagesInConversationUseCase getMessagesInConversationUseCase;
        private readonly IMarkMessageAsDeliveredUseCase markMessageAsDeliveredUseCase;
        private readonly IMarkNewMessagesAsSeenUseCase markNewMessagesAsSeenUseCase;
        private readonly IRemoveReactionFromMessageUseCase removeReactionFromMessageUseCase;
        private readonly ISendMessageUseCase sendMessageUseCase;

        public MessagesController
            (
            IAddReactionToMessageUseCase addReactionToMessageUseCase,
            IDeleteMessageUseCase deleteMessageUseCase,
            IEditMessageUseCase editMessageUseCase,
            IGetMessagesInConversationUseCase getMessagesInConversationUseCase,
            IMarkMessageAsDeliveredUseCase markMessageAsDeliveredUseCase,
            IMarkNewMessagesAsSeenUseCase markNewMessagesAsSeenUseCase,
            IRemoveReactionFromMessageUseCase removeReactionFromMessageUseCase,
            ISendMessageUseCase sendMessageUseCase

            )
        {
            this.addReactionToMessageUseCase = addReactionToMessageUseCase;
            this.deleteMessageUseCase = deleteMessageUseCase;
            this.editMessageUseCase = editMessageUseCase;
            this.getMessagesInConversationUseCase = getMessagesInConversationUseCase;
            this.markMessageAsDeliveredUseCase = markMessageAsDeliveredUseCase;
            this.markNewMessagesAsSeenUseCase = markNewMessagesAsSeenUseCase;
            this.removeReactionFromMessageUseCase = removeReactionFromMessageUseCase;
            this.sendMessageUseCase = sendMessageUseCase;
        }

        [HttpGet("conversation")]
        public async Task<IActionResult> GetMessagesInConversation([FromBody] GetConversationDto dto)
        {
            var messages = await getMessagesInConversationUseCase.ExecuteAsync(dto.conversationId, dto.pageIndex, dto.pageSize);
            return Ok(messages);
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromQuery] Guid senderId, [FromQuery] Guid conversationId, [FromBody] string content)
        {
            var messageId = await sendMessageUseCase.ExecuteAsync(senderId, conversationId, content);
            return Ok(messageId);
        }

        [HttpPut("{messageId}/edit")]
        public async Task<IActionResult> EditMessage(Guid messageId, [FromBody] string newContent)
        {
            await editMessageUseCase.ExecuteAsync(messageId, newContent);
            return NoContent();
        }

        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMessage(Guid messageId)
        {
            await deleteMessageUseCase.ExecuteAsync(messageId);
            return NoContent();
        }

        [HttpPost("{messageId}/reaction")]
        public async Task<IActionResult> AddReaction(Guid messageId, [FromQuery] Guid userId, [FromQuery] Guid reactionId)
        {
            await addReactionToMessageUseCase.ExecuteAsync(messageId, userId, reactionId);
            return NoContent();
        }

        [HttpDelete("{messageId}/reaction")]
        public async Task<IActionResult> RemoveReaction(Guid messageId, [FromQuery] Guid userId)
        {
            await removeReactionFromMessageUseCase.ExecuteAsync(messageId, userId);
            return NoContent();
        }

        [HttpPut("{messageId}/delivered")]
        public async Task<IActionResult> MarkAsDelivered(Guid messageId)
        {
            await markMessageAsDeliveredUseCase.ExecuteAsync(messageId);
            return NoContent();
        }

        [HttpPut("conversation/{conversationId}/seen")]
        public async Task<IActionResult> MarkNewMessagesAsSeen(Guid conversationId, [FromQuery] Guid userId)
        {
            await markNewMessagesAsSeenUseCase.ExecuteAsync(conversationId, userId);
            return NoContent();
        }
    }
}
