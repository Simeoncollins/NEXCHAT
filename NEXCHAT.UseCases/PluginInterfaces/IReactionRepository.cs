using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.PluginInterfaces
{
    public interface IReactionRepository
    {
        Task<Guid> CreateReactionAsync(string reactionName, string emojiPath);
        Task<Reaction> GetReactionByIdAsync(Guid reactionId);
        Task<IEnumerable<Reaction>> GetReactionsAsync();
    }
}
