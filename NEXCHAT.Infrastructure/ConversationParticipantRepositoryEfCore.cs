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
            // attempting to return all user conversations including their last message and unread messages count.
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var conversations = await context.ConversationParticipants
                .Where(cp => cp.UserId == userId)
                .Select(cp => new  // Project to temporary object
                {
                    Conversation = cp.Conversation,
                    LastMessage = cp.Conversation.Messages
                        .OrderByDescending(m => m.DateSentUTC)
                        .FirstOrDefault(),
                    UnreadCount = cp.Conversation.Messages
                        .Count(m => !m.SeenBy.Any(ms => ms.UserId == userId))
                })
                .Where(x => x.Conversation.IsGroupConversation == isGroupConversation)
                .AsNoTracking()
                .ToListAsync();

            // Map calculated values back to Conversation entities
            foreach (var item in conversations)
            {
                item.Conversation.UnreadMessagesCount = item.UnreadCount;
                item.Conversation.LastMessage = item.LastMessage;
            }

            return conversations.Select(x => x.Conversation).ToList();
        }
    }
}
