using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness;
using NEXCHAT.UseCases.MessageManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.MessageManagement
{
    public class GetMessagesInConversationUseCase : IGetMessagesInConversationUseCase
    {
        
        private readonly IMessageRepository iMessageRepository;

        public GetMessagesInConversationUseCase(IMessageRepository iMessageRepository)
        {
            this.iMessageRepository = iMessageRepository;
        }

        public async Task<IEnumerable<Message>> ExecuteAsync(Guid conversationId, int page, int pageSize)
        {
            return await iMessageRepository.GetMessagesInConversationAsync(conversationId, page, pageSize);
        }
    }
}
