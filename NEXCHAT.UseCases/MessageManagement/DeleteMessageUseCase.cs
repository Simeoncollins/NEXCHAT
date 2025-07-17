using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.MessageManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.MessageManagement
{
    public class DeleteMessageUseCase : IDeleteMessageUseCase
    {
        private readonly IMessageRepository iMessageRepository;

        public DeleteMessageUseCase(IMessageRepository iMessageRepository)
        {
            this.iMessageRepository = iMessageRepository;
        }

        public async Task ExecuteAsync(Guid messageId)
        {
            await iMessageRepository.DeleteMessageAsync(messageId);
        }
    }
}
