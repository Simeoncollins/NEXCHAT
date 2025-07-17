using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.FriendManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.FriendManagement
{
    public class RejectFriendRequestUseCase : IRejectFriendRequestUseCase
    {
        private readonly IUserRepository iUserRepository;

        public RejectFriendRequestUseCase(IUserRepository iUserRepository)
        {
            this.iUserRepository = iUserRepository;
        }

        public async Task ExecuteAsync(Guid friendRequestId)
        {
            await iUserRepository.RejectFriendRequestAsync(friendRequestId);
        }
    }
}
