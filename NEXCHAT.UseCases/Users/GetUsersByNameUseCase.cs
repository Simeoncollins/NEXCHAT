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
    public class GetUsersByNameUseCase : IGetUsersByNameUseCase
    {
        private readonly IUserRepository userRepository;

        public GetUsersByNameUseCase(IUserRepository UserRepository)
        {
            userRepository = UserRepository;
        }

        public async Task<IEnumerable<User>> ExecuteAsync(string name = "", int pageIndex = 1, int pageSize = 50)
        {
            return await userRepository.GetUsersByNameAsync(name, pageIndex, pageSize);
        }
    }
}
