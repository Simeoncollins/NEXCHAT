using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.ConversationManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.ConversationManagement
{
    public class AddParticipantToConversationUseCase : IAddParticipantToConversationUseCase
    {
        private readonly IConversationRepository iConversationRepository;

        public AddParticipantToConversationUseCase(IConversationRepository iConversationRepository)
        {
            this.iConversationRepository = iConversationRepository;
        }

        public async Task ExecuteAsync(Guid conversationId, Guid userId)
        {
            await iConversationRepository.AddParticipantToConversationAsync(conversationId, userId);
        }
    }
}
