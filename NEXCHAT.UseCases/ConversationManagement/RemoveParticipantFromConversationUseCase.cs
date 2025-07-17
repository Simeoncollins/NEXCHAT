using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.ConversationManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.ConversationManagement
{
    public class RemoveParticipantFromConversationUseCase : IRemoveParticipantFromConversationUseCase
    {
        private readonly IConversationRepository iConversationRepository;

        public RemoveParticipantFromConversationUseCase(IConversationRepository iConversationRepository)
        {
            this.iConversationRepository = iConversationRepository;
        }

        public async Task ExecuteAsync(Guid conversationId, Guid userId)
        {
            await iConversationRepository.RemoveParticipantFromConversationAsync(conversationId, userId);
        }
    }
}
