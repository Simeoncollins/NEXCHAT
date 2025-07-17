using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.PluginInterfaces
{
    public interface IConversationRepository
    {
        Task AddParticipantToConversationAsync(Guid conversationId, Guid userId);
        Task<Conversation?> GetConversationAsync(Guid conversationId);
        Task RemoveParticipantFromConversationAsync(Guid conversationId, Guid userId);
        Task SetTypingIndicatorAsync(Guid conversationId, Guid userId, bool isTyping);
        Task<Guid> StartConversationAsync(Conversation conversation);
        Task UpdateGroupDetailsAsync(Guid conversationId, string groupName, string groupCoverPhotoPath);
    }
}
