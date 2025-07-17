using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness;
using NEXCHAT.UseCases.ConversationManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.ConversationManagement
{
    public class GetUserConversationsUseCase : IGetUserConversationsUseCase
    {
        private readonly IConversationParticipantRepository iConversationParticipantRepository;

        public GetUserConversationsUseCase(IConversationParticipantRepository iConversationParticipantRepository)
        {
            this.iConversationParticipantRepository = iConversationParticipantRepository;
        }

        public async Task<IEnumerable<Conversation>> ExecuteAsync(Guid userId, bool isGroupConversation = false)
        {
            return await iConversationParticipantRepository.GetUserConversationsAsync(userId, isGroupConversation);
        }
    }
}
