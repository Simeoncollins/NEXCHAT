using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.PluginInterfaces
{
    public interface IConversationParticipantRepository
    {
        Task<int> GetUnreadConversationCountAsync(Guid userId, bool isGroup);
        Task<IEnumerable<Conversation>> GetUserConversationsAsync(Guid userId, bool isGroupConversation);
    }
}
