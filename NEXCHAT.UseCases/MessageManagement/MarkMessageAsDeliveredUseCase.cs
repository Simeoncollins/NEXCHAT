using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.MessageManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.MessageManagement
{
    public class MarkMessageAsDeliveredUseCase : IMarkMessageAsDeliveredUseCase
    {
        private readonly IMessageRepository iMessageRepository;

        public MarkMessageAsDeliveredUseCase(IMessageRepository iMessageRepository)
        {
            this.iMessageRepository = iMessageRepository;
        }

        public async Task ExecuteAsync(Guid messageId)
        {
            await iMessageRepository.MarkMessageAsDeliveredAsync(messageId);
        }
    }
}
