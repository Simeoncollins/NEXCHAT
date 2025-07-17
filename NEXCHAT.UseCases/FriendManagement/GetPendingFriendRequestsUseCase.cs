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
    public class GetPendingFriendRequestsUseCase : IGetPendingFriendRequestsUseCase
    {
        private readonly IUserRepository iUserRepository;

        public GetPendingFriendRequestsUseCase(IUserRepository iUserRepository)
        {
            this.iUserRepository = iUserRepository;
        }

        public async Task<IEnumerable<User>> ExecuteAsync(Guid userId)
        {
            return await iUserRepository.GetPendingFriendRequestsAsync(userId);
        }
    }
}
