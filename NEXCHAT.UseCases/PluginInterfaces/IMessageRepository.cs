using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.PluginInterfaces
{
    public interface IMessageRepository
    {
        Task AddReactionToMessageAsync(Guid messageId, Guid userId, Guid reactionId);
        Task DeleteMessageAsync(Guid messageId);
        Task EditMessageAsync(Guid messageId, string newContent);
        Task<IEnumerable<Message>> GetMessagesInConversationAsync(Guid conversationId, int page, int pageSize);
        Task MarkMessageAsDeliveredAsync(Guid messageId);
        Task RemoveReactionFromMessageAsync(Guid messageId, Guid userId);
        Task<Guid> SendMessageAsync(Message message);
        Task MarkNewMessagesAsSeenAsync(Guid conversationId, Guid userId);
    }
}
