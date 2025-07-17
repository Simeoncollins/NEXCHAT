using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.ConversationManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.ConversationManagement
{
    public class UpdateGroupDetailsUseCase : IUpdateGroupDetailsUseCase
    {
        private readonly IConversationRepository iConversationRepository;

        public UpdateGroupDetailsUseCase(IConversationRepository iConversationRepository)
        {
            this.iConversationRepository = iConversationRepository;
        }

        public async Task ExecuteAsync(Guid conversationId, string groupName = "", string groupCoverPhotoPath = "")
        {
            await iConversationRepository.UpdateGroupDetailsAsync(conversationId, groupName, groupCoverPhotoPath);
        }
    }
}
