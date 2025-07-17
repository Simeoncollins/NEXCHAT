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
    public class StartConversationUseCase : IStartConversationUseCase
    {
        private readonly IConversationRepository iConversationRepository;

        public StartConversationUseCase(IConversationRepository iConversationRepository)
        {
            this.iConversationRepository = iConversationRepository;
        }

        public async Task<Guid> ExecuteAsync(Guid creatorId, List<Guid> initialParticipantsId, bool isGroup, string groupName = "")
        {
            var conversationParticipants = new List<ConversationParticipant>();
            var conversationId = Guid.NewGuid();
            foreach (var participant in initialParticipantsId)
            {
                conversationParticipants.Add(new ConversationParticipant()
                {
                    UserId = creatorId,
                    ConversationId = conversationId
                });
            }
            Conversation conversation = new Conversation()
            {
                ConversationId = conversationId,
                CreatorId = creatorId,
                DateStartedUTC = DateTime.UtcNow,
                GroupName = groupName,
                ConversationParticipants = conversationParticipants
            };
            return await iConversationRepository.StartConversationAsync(conversation);
        }
    }
}
