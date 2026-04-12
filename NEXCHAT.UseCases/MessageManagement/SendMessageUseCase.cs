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
    public class SendMessageUseCase : ISendMessageUseCase
    {
        private readonly IMessageRepository iMessageRepository;

        public SendMessageUseCase(IMessageRepository iMessageRepository)
        {
            this.iMessageRepository = iMessageRepository;
        }

        public async Task<Message> ExecuteAsync(Message message)
        {
            await iMessageRepository.SendMessageAsync(message);
            return message;
        }
    }
}
