using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness;
using NEXCHAT.UseCases.PluginInterfaces;
using NEXCHAT.UseCases.Users.Interfaces;

namespace NEXCHAT.UseCases.Users
{
    public class GetUserByIdUseCase : IGetUserByIdUseCase
    {
        private readonly IUserRepository iUserRepository;

        public GetUserByIdUseCase(IUserRepository iUserRepository)
        {
            this.iUserRepository = iUserRepository;
        }

        public async Task<User> ExecuteAsync(Guid userId)
        {
            return await iUserRepository.GetUserByIdAsync(userId);
        }
    }
}
