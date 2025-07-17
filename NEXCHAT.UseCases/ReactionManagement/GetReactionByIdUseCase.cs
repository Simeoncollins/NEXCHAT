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
    public class GetReactionByIdUseCase : IGetReactionByIdUseCase
    {
        private readonly IReactionRepository iReactionRepository;

        public GetReactionByIdUseCase(IReactionRepository iReactionRepository)
        {
            this.iReactionRepository = iReactionRepository;
        }

        public async Task<Reaction> ExecuteAsync(Guid reactionId)
        {
            return await iReactionRepository.GetReactionByIdAsync(reactionId);
        }
    }
}
