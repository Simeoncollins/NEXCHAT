using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.FriendManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.FriendManagement
{
    public class AcceptFriendRequestUseCase : IAcceptFriendRequestUseCase
    {
        private readonly IUserRepository iUserRepository;

        public AcceptFriendRequestUseCase(IUserRepository iUserRepository)
        {
            this.iUserRepository = iUserRepository;
        }

        public async Task ExecuteAsync(Guid requesterId, Guid receiverId)
        {
            await iUserRepository.AcceptFriendRequestAsync(requesterId, receiverId);
        }
    }
}
