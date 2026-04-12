using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NEXCHAT.CoreBusiness.Classes;
using NEXCHAT.CoreBusiness;
using NEXCHAT.UseCases.PluginInterfaces;
using NEXCHAT.CoreBusiness.Interfaces;
using Microsoft.VisualBasic;
using static System.Collections.Specialized.BitVector32;
using NEXCHAT.Infrastructure.Data;

namespace NEXCHAT.Infrastructure.Repositories
{
    public class MessageRepositoryEfCore : IMessageRepository
    {
        private readonly IDbContextFactory<NEXCHATDBContext> _dbContextFactory;
        private readonly IRealTimeNotifier _notifier;
        private readonly IConversationRepository _conversationRepository;
        private readonly IReactionRepository _reactionRepository;

        public MessageRepositoryEfCore(
            IDbContextFactory<NEXCHATDBContext> dbContextFactory,
            IRealTimeNotifier notifier,
            IConversationRepository conversationRepository,
            IReactionRepository reactionRepository)
        {
            _dbContextFactory = dbContextFactory;
            _notifier = notifier;
            _conversationRepository = conversationRepository;
            _reactionRepository = reactionRepository;
        }

        public async Task AddReactionToMessageAsync(Guid messageId, Guid userId, Guid reactionId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var message = await context.Messages
                .Include(m => m.Reactions)
                    .ThenInclude(r => r.Reaction)
                .FirstOrDefaultAsync(m => m.MessageId == messageId);

            if (message == null) return;
            var reaction = await _reactionRepository.GetReactionByIdAsync(reactionId);
            var conversation = await _conversationRepository.GetConversationAsync(message.ConversationId);

            var existingReaction = message.Reactions.FirstOrDefault(r => r.UserReactedId == userId);
            if (existingReaction != null)
            {
                existingReaction.ReactionId = reactionId;
                existingReaction.Reaction = reaction;
            }
            else
            {
                message.Reactions.Add(new MessageReaction
                {
                    ReactionId = reactionId,
                    UserReactedId = userId,
                    MessageId = messageId,
                    Reaction = reaction
                });
            }

            await context.SaveChangesAsync();

            foreach (var r in message.Reactions)
            {
                r.Message = null; // break circular reference for SignalR JSON serializer
            }
            message.Conversation = null;
            message.Sender = null;

            foreach (var participant in conversation.ConversationParticipants)
            {
                await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "MessageReacted", new ReactionEvent(message, reaction));
            }
        }

        public async Task DeleteMessageAsync(Guid messageId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var message = await context.Messages.FindAsync(messageId);
            if (message == null) return;
            var conversation = await _conversationRepository.GetConversationAsync(message.ConversationId);

            message.Content = "This message was deleted";
            message.IsDeleted = true;
            message.ModifiedAt = DateTime.UtcNow;
            // Clear reactions by removing them
            await context.MessageReactions
                .Where(r => r.MessageId == messageId)
                .ExecuteDeleteAsync();

            await context.SaveChangesAsync();

            // Broadcast to ALL participants (including sender) so every client updates
            foreach (var participant in conversation.ConversationParticipants)
                await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "MessageDeleted", messageId);
        }

        public async Task EditMessageAsync(Guid messageId, string newContent)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var message = await context.Messages
                .Include(m => m.SeenBy)
                .Include(m => m.Reactions).ThenInclude(r => r.Reaction)
                .FirstOrDefaultAsync(m => m.MessageId == messageId);
            if (message == null) return;
            var conversation = await _conversationRepository.GetConversationAsync(message.ConversationId);
            message.Content = newContent;
            message.IsEdited = true;
            message.ModifiedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();

            // Broadcast to ALL participants (including sender) so every client updates
            foreach (var participant in conversation.ConversationParticipants)
                await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "MessageEdited", message);
        }

        public async Task<IEnumerable<Message>> GetMessagesInConversationAsync(Guid conversationId, int page, int pageSize)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var baseQuery = context.Messages
                .Where(m => m.ConversationId == conversationId)  // include deleted so bubble shows "This message was deleted"
                .OrderBy(m => m.DateSentUTC)
                .Include(m => m.SeenBy)
                .Include(m => m.Reactions)
                    .ThenInclude(r => r.Reaction);

            var messages = await baseQuery
                .AsNoTracking()
                .ToListAsync();


            return messages;
        }

        public async Task RemoveReactionFromMessageAsync(Guid messageId, Guid userId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var message = await context.Messages
                .Include(m => m.Reactions)
                    .ThenInclude(r => r.Reaction)
                .FirstOrDefaultAsync(m => m.MessageId == messageId);

            if (message == null) return;
            var conversation = await _conversationRepository.GetConversationAsync(message.ConversationId);

            var existingReaction = message.Reactions.FirstOrDefault(r => r.UserReactedId == userId);
            if (existingReaction != null)
            {
                message.Reactions.Remove(existingReaction);
                await context.SaveChangesAsync();
                
                foreach (var r in message.Reactions)
                {
                    r.Message = null; // break circular reference
                }
                message.Conversation = null;
                message.Sender = null;

                foreach (var participant in conversation.ConversationParticipants)
                {
                    await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "MessageReacted", new ReactionEvent(message, new Reaction()));
                }
            }
        }

        public async Task<Guid> SendMessageAsync(Message message)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            context.Messages.Add(message);
            await context.SaveChangesAsync();

            // Load the Sender so the client toast can display the name
            await context.Entry(message).Reference(m => m.Sender).LoadAsync();

            var conversation = await _conversationRepository.GetConversationAsync(message.ConversationId);
            foreach (var participant in conversation.ConversationParticipants)
            {
                if (participant.UserId != message.SenderId)
                    await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "MessageReceived", message);
            }

            return message.MessageId;
        }

        public async Task MarkMessageAsDeliveredAsync(Guid userId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            // Get user's conversation IDs
            var userConversationIds = await context.ConversationParticipants
                .Where(p => p.UserId == userId)
                .Select(p => p.ConversationId)
                .ToListAsync();

            if (!userConversationIds.Any()) return;

            // Update messages in bulk
            await context.Messages
                .Where(m => m.SenderId != userId)
                .Where(m => userConversationIds.Contains(m.ConversationId))
                .Where(m => !m.IsDelivered)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(m => m.IsDelivered, true));

            // Get conversations to notify
            var conversations = await context.Conversations
                .Include(c => c.ConversationParticipants)
                .Where(c => userConversationIds.Contains(c.ConversationId))
                .ToListAsync();

            foreach (var conversation in conversations)
            {
                foreach (var participant in conversation.ConversationParticipants)
                {
                    if (participant.UserId != userId)
                    {
                        await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "MessageDelivered", conversation.ConversationId);
                    }
                }
            }
        }

        public async Task MarkNewMessagesAsSeenAsync(Guid conversationId, Guid userId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            // Only mark messages sent by others (not by the user themselves)
            var unseenMessages = await context.Messages
                .Where(m => m.ConversationId == conversationId)
                .Where(m => m.SenderId != userId)    // exclude own messages
                .Where(m => !m.SeenBy.Any(ms => ms.UserId == userId))
                .ToListAsync();

            if (!unseenMessages.Any()) return;

            var seenEntries = unseenMessages.Select(m => new MessageSeen
            {
                MessageId = m.MessageId,
                UserId = userId,
                SeenAtUTC = DateTime.UtcNow
            }).ToList();

            await context.MessagesSeen.AddRangeAsync(seenEntries);
            await context.SaveChangesAsync();

            var conversation = await _conversationRepository.GetConversationAsync(conversationId);
            foreach (var participant in conversation.ConversationParticipants)
            {
                if (participant.UserId != userId)
                {
                    // Send each seen entry individually so the client event handler receives a single MessageSeen
                    foreach (var entry in seenEntries)
                        await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "MessagesSeen", entry);
                }
            }
        }

    }
}
