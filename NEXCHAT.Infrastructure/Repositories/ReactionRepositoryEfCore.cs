using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NEXCHAT.CoreBusiness;
using NEXCHAT.Infrastructure.Data;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.Infrastructure.Repositories
{
    public class ReactionRepositoryEfCore : IReactionRepository
    {
        private readonly IDbContextFactory<NEXCHATDBContext> _dbContextFactory;

        public ReactionRepositoryEfCore(IDbContextFactory<NEXCHATDBContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<Guid> CreateReactionAsync(string reactionName, string emojiPath)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var exists = await context.Reactions.AnyAsync(r => r.ReactionName == reactionName);
            if (exists) throw new InvalidOperationException("Reaction name already exists");

            var reaction = new Reaction
            {
                ReactionId = Guid.NewGuid(),
                ReactionName = reactionName,
                EmojiPath = emojiPath,
            };

            await context.Reactions.AddAsync(reaction);
            await context.SaveChangesAsync();
            return reaction.ReactionId;
        }

        public async Task<Reaction> GetReactionByIdAsync(Guid reactionId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Reactions
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.ReactionId == reactionId);
        }

        public async Task<IEnumerable<Reaction>> GetReactionsAsync()
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Reactions
                .OrderBy(r => r.ReactionName)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
