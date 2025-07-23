using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness;
using NEXCHAT.CoreBusiness.Classes;
using NEXCHAT.UseCases.FriendManagement.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.UseCases.FriendManagement
{
    public class GetFriendListUseCase : IGetFriendListUseCase
    {
        private readonly IUserRepository iUserRepository;

        public GetFriendListUseCase(IUserRepository iUserRepository)
        {
            this.iUserRepository = iUserRepository;
        }

        public async Task<IEnumerable<FriendDto>> ExecuteAsync(Guid userId)
        {
            return await iUserRepository.GetFriendListAsync(userId);
        }
    }
}
