using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.MessageManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.MessageManagement
{
    public class RemoveReactionFromMessageUseCase : IRemoveReactionFromMessageUseCase
    {
        private readonly IMessageRepository iMessageRepository;

        public RemoveReactionFromMessageUseCase(IMessageRepository iMessageRepository)
        {
            this.iMessageRepository = iMessageRepository;
        }

        public async Task ExecuteAsync(Guid messageId, Guid userId)
        {
            await iMessageRepository.RemoveReactionFromMessageAsync(messageId, userId);
        }
    }
}
