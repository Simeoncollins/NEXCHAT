using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.ConversationManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.ConversationManagement
{
    public class SetTypingIndicatorUseCase : ISetTypingIndicatorUseCase
    {
        private readonly IConversationRepository iConversationRepository;

        public SetTypingIndicatorUseCase(IConversationRepository iConversationRepository)
        {
            this.iConversationRepository = iConversationRepository;
        }

        public async Task ExecuteAsync(Guid conversationId, Guid userId, bool isTyping)
        {
            await iConversationRepository.SetTypingIndicatorAsync(conversationId, userId, isTyping);
        }
    }
}
