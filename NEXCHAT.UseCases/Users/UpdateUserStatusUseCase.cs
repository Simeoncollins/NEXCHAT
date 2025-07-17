using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness.Enums;
using NEXCHAT.UseCases.PluginInterfaces;
using NEXCHAT.UseCases.Users.Interfaces;

namespace NEXCHAT.UseCases.Users
{
    public class UpdateUserStatusUseCase : IUpdateUserStatusUseCase
    {
        private readonly IUserRepository iUserRepository;

        public UpdateUserStatusUseCase(IUserRepository iUserRepository)
        {
            this.iUserRepository = iUserRepository;
        }

        public async Task ExecuteAsync(Guid userId, StatusType statusType)
        {
            await iUserRepository.UpdateStatusAsync(userId, statusType);
        }
    }
}
