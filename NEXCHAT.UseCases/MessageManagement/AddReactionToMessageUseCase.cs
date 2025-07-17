using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.MessageManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.MessageManagement
{
    public class AddReactionToMessageUseCase : IAddReactionToMessageUseCase
    {
        private readonly IMessageRepository iMessageRepository;

        public AddReactionToMessageUseCase(IMessageRepository iMessageRepository)
        {
            this.iMessageRepository = iMessageRepository;
        }

        public async Task ExecuteAsync(Guid messageId, Guid userId, Guid reactionId)
        {
            await iMessageRepository.AddReactionToMessageAsync(messageId, userId, reactionId);
        }
    }
}
