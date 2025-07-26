using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NEXCHAT.CoreBusiness;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.Plugin.EFCore
{
    public class ConversationParticipantRepositoryEfCore : IConversationParticipantRepository
    {
        private readonly IDbContextFactory<NEXCHATDBContext> _dbContextFactory;

        public ConversationParticipantRepositoryEfCore(IDbContextFactory<NEXCHATDBContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<int> GetUnreadConversationCountAsync(Guid userId, bool isGroup)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            return await context.ConversationParticipants
                .Where(cp => cp.UserId == userId)
                .Select(cp => cp.Conversation)
                .Where(c => c.IsGroupConversation == isGroup)
                .CountAsync(c => c.Messages.Any(m =>
                    !m.SeenBy.Any(ms => ms.UserId == userId)
                ));
        }

        public async Task<IEnumerable<Conversation>> GetUserConversationsAsync(Guid userId, bool isGroupConversation)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            // Get conversations where user is a participant or the creator
            var conversations = await context.Conversations
                .Where(c =>
                    c.IsGroupConversation == isGroupConversation &&
                    (c.CreatorId == userId || c.ConversationParticipants.Any(cp => cp.UserId == userId)))
                .Include(c => c.ConversationParticipants)
                    .ThenInclude(cp => cp.User)
                .Include(c => c.Creator)
                .Include(c => c.Messages)
                    .ThenInclude(m => m.SeenBy)
                .AsNoTracking()
                .ToListAsync();

            foreach (var conversation in conversations)
            {
                // Set last message
                conversation.LastMessage = conversation.Messages
                    .OrderByDescending(m => m.DateSentUTC)
                    .FirstOrDefault();

                // Set unread message count
                conversation.UnreadMessagesCount = conversation.Messages
                    .Count(m => !m.SeenBy.Any(sb => sb.UserId == userId));
            }

            return conversations;
        }

    }
}
