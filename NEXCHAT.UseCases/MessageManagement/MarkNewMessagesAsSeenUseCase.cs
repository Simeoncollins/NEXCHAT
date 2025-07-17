using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.MessageManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.MessageManagement
{
    public class MarkNewMessagesAsSeenUseCase : IMarkNewMessagesAsSeenUseCase
    {
        private readonly IMessageRepository iMessageRepository;

        public MarkNewMessagesAsSeenUseCase(IMessageRepository iMessageRepository)
        {
            this.iMessageRepository = iMessageRepository;
        }

        public async Task ExecuteAsync(Guid conversationId, Guid userId)
        {
            await iMessageRepository.MarkNewMessagesAsSeenAsync(conversationId, userId);
        }
    }
}
