using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NEXCHAT.CoreBusiness;
using NEXCHAT.CoreBusiness.Interfaces;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.Plugin.EFCore
{
    public class ConversationRepositoryEfCore : IConversationRepository
    {
        private readonly IDbContextFactory<NEXCHATDBContext> _dbContextFactory;
        private readonly IRealTimeNotifier _notifier;

        public ConversationRepositoryEfCore(IDbContextFactory<NEXCHATDBContext> dbContextFactory, IRealTimeNotifier notifier)
        {
            _dbContextFactory = dbContextFactory;
            _notifier = notifier;
        }

        public async Task AddParticipantToConversationAsync(Guid conversationId, Guid userId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            var participant = new ConversationParticipant
            {
                ConversationId = conversationId,
                UserId = userId
            };

            await context.ConversationParticipants.AddAsync(participant);
            await context.SaveChangesAsync();
        }

        public async Task<Conversation?> GetConversationAsync(Guid conversationId)
        {
            // does not include messages
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            return await context.Conversations
                .Include(c => c.ConversationParticipants)
                    .ThenInclude(cp => cp.User) // Include user details if needed
                .Include(c => c.ParticipantsTyping)
                .AsNoTracking() // Recommended for read-only
                .FirstOrDefaultAsync(c => c.ConversationId == conversationId);
        }

        public async Task RemoveParticipantFromConversationAsync(Guid conversationId, Guid userId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            var participant = await context.ConversationParticipants
                .FirstOrDefaultAsync(cp =>
                    cp.ConversationId == conversationId &&
                    cp.UserId == userId);

            if (participant != null)
            {
                context.ConversationParticipants.Remove(participant);
                await context.SaveChangesAsync();
            }
        }

        public async Task SetTypingIndicatorAsync(Guid conversationId, Guid userId, bool isTyping)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            var typingUser = await context.ConversationTypingUsers
                .FirstOrDefaultAsync(t =>
                    t.ConversationId == conversationId &&
                    t.UserId == userId);
            var conversataion = await context.Conversations
                .Include(c => c.ConversationParticipants)
                .FirstOrDefaultAsync(c => c.ConversationId == conversationId);

            if (isTyping)
            {
                if (typingUser == null)
                {
                    await context.ConversationTypingUsers.AddAsync(new ConversationTypingUser
                    {
                        ConversationId = conversationId,
                        UserId = userId,
                    });
                    await context.SaveChangesAsync();
                }
            }
            else
            {
                if (typingUser != null)
                {
                    context.ConversationTypingUsers.Remove(typingUser);
                    await context.SaveChangesAsync();
                }
            }
            if (conversataion != null)
            {
                foreach (var participant in conversataion.ConversationParticipants)
                {
                    await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "TypingUpdate", true);
                }
            }
        }

        public async Task<Guid> StartConversationAsync(Conversation conversation)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            await context.Conversations.AddAsync(conversation);
            await context.SaveChangesAsync();
            return conversation.ConversationId;
        }

        public async Task UpdateGroupDetailsAsync(Guid conversationId, string groupName, string groupCoverPhotoPath)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            var conversation = await context.Conversations.FindAsync(conversationId);
            if (conversation == null) return;

            if (!string.IsNullOrEmpty(groupName))
                conversation.GroupName = groupName;

            if (!string.IsNullOrEmpty(groupCoverPhotoPath))
                conversation.GroupCoverPhotoPath = groupCoverPhotoPath;

            context.Conversations.Update(conversation);
            await context.SaveChangesAsync();
        }
    }
}
