using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.ConversationManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.ConversationManagement
{
    public class GetUnreadConversationCountUseCase : IGetUnreadConversationCountUseCase
    {
        private readonly IConversationParticipantRepository iConversationParticipantRepository;

        public GetUnreadConversationCountUseCase(IConversationParticipantRepository iConversationRepository)
        {
            this.iConversationParticipantRepository = iConversationRepository;
        }

        public async Task<int> ExecuteAsync(Guid userId, bool isGroup)
        {
            return await iConversationParticipantRepository.GetUnreadConversationCountAsync(userId, isGroup);
        }
    }
}
