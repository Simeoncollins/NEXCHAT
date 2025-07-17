using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.FriendManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.FriendManagement
{
    public class BlockFriendUseCase : IBlockFriendUseCase
    {
        private readonly IUserRepository iUserRepository;

        public BlockFriendUseCase(IUserRepository iUserRepository)
        {
            this.iUserRepository = iUserRepository;
        }

        public async Task ExecuteAsync(Guid userId, Guid friendId)
        {
            await iUserRepository.BlockFriendAsync(userId, friendId);
        }
    }
}
