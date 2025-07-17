using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.UseCases.PluginInterfaces;
using NEXCHAT.UseCases.ReactionManagement.Interfaces;

namespace NEXCHAT.UseCases.ReactionManagement
{
    public class CreateReactionUseCase : ICreateReactionUseCase
    {
        private readonly IReactionRepository iReactionRepository;

        public CreateReactionUseCase(IReactionRepository iReactionRepository)
        {
            this.iReactionRepository = iReactionRepository;
        }

        public async Task<Guid> ExecuteAsync(string reactionName, string emojiPath)
        {
            return await iReactionRepository.CreateReactionAsync(reactionName, emojiPath);
        }
    }
}
