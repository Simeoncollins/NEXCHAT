using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.FriendManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.FriendManagement
{
    public class CheckIfBlockedByFriendUseCase : ICheckIfBlockedByFriendUseCase
    {
        private readonly IUserRepository iUserRepository;

        public CheckIfBlockedByFriendUseCase(IUserRepository userRepository)
        {
            this.iUserRepository = userRepository;
        }

        public async Task<bool> ExecuteAsync(Guid userId, Guid friendId)
        {
            return await iUserRepository.IsBlockedByFriendAsync(userId, friendId);
        }
    }
}
