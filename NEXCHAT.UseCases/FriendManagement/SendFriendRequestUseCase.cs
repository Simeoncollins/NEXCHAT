using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.FriendManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.FriendManagement
{
    public class SendFriendRequestUseCase : ISendFriendRequestUseCase
    {
        private readonly IUserRepository iUserRepository;

        public SendFriendRequestUseCase(IUserRepository iUserRepository)
        {
            this.iUserRepository = iUserRepository;
        }

        public async Task ExecuteAsync(Guid requesterId, Guid receiverId)
        {
            await iUserRepository.SendFriendRequestAsync(requesterId, receiverId);
        }
    }
}
