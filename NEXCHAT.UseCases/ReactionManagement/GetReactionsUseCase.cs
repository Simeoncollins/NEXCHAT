using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness;
using NEXCHAT.UseCases.PluginInterfaces;
using NEXCHAT.UseCases.ReactionManagement.Interfaces;

namespace NEXCHAT.UseCases.ReactionManagement
{
    public class GetReactionsUseCase : IGetReactionsUseCase
    {
        private readonly IReactionRepository iReactionRepository;

        public GetReactionsUseCase(IReactionRepository iReactionRepository)
        {
            this.iReactionRepository = iReactionRepository;
        }

        public async Task<IEnumerable<Reaction>> ExecuteAsync()
        {
            return await iReactionRepository.GetReactionsAsync();
        }
    }
}
