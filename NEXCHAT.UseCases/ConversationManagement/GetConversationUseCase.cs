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
    public class GetConversationUseCase : IGetConversationUseCase
    {
        private readonly IConversationRepository iConversationRepository;

        public GetConversationUseCase(IConversationRepository iConversationRepository)
        {
            this.iConversationRepository = iConversationRepository;
        }

        public async Task<Conversation?> ExecuteAsync(Guid conversationId)
        {
            return await iConversationRepository.GetConversationAsync(conversationId);
        }
    }
}
