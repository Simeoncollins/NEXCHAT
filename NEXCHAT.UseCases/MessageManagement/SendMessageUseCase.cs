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

        public async Task<Guid> ExecuteAsync(Guid senderId, Guid conversationId, string content)
        {
            var messageId = Guid.NewGuid();
            var message = new Message
            {
                MessageId = messageId,
                Content = content,
                ConversationId = conversationId,
                SenderId = senderId,
                DateSentUTC = DateTime.UtcNow,
            };
            return await iMessageRepository.SendMessageAsync(message);
        }
    }
}
