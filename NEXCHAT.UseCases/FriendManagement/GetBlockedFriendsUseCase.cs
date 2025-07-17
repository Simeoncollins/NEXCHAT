using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness;
using NEXCHAT.UseCases.FriendManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.FriendManagement
{
    public class GetBlockedFriendsUseCase : IGetBlockedFriendsUseCase
    {
        private readonly IUserRepository iUserRepository;

        public GetBlockedFriendsUseCase(IUserRepository iUserRepository)
        {
            this.iUserRepository = iUserRepository;
        }

        public async Task<IEnumerable<User>> ExecuteAsync(Guid userId)
        {
            return await iUserRepository.GetBlockedFriendsAsync(userId);
        }
    }
}
