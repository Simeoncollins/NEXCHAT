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

namespace NEXCHAT.Plugin.EFCore
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
                .FirstOrDefaultAsync(m => m.MessageId == messageId);

            if (message == null) return;
            var reaction = await _reactionRepository.GetReactionByIdAsync(reactionId);
            var conversation = await _conversationRepository.GetConversationAsync(message.ConversationId);

            var existingReaction = message.Reactions.FirstOrDefault(r => r.UserReactedId == userId);
            if (existingReaction != null)
            {
                existingReaction.ReactionId = reactionId;
            }
            else
            {
                message.Reactions.Add(new MessageReaction
                {
                    ReactionId = reactionId,
                    UserReactedId = userId,
                    MessageId = messageId
                });
            }

            await context.SaveChangesAsync();

            foreach (var participant in conversation.ConversationParticipants)
            {
                if (participant.UserId != userId)
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

            await context.SaveChangesAsync();
            foreach (var participant in conversation.ConversationParticipants)
            {
                if (participant.UserId != message.SenderId)
                    await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "MessageDeleted", true);
            }
        }

        public async Task EditMessageAsync(Guid messageId, string newContent)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var message = await context.Messages.FindAsync(messageId);
            if (message == null) return;
            var conversation = await _conversationRepository.GetConversationAsync(message.ConversationId);
            message.Content = newContent;
            message.IsEdited = true;
            message.ModifiedAt = DateTime.UtcNow;

            await context.SaveChangesAsync();
            foreach (var participant in conversation.ConversationParticipants)
            {
                if (participant.UserId != message.SenderId)
                    await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "MessageEdited", true);
            }
        }

        public async Task<IEnumerable<Message>> GetMessagesInConversationAsync(Guid conversationId, int page, int pageSize)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var baseQuery = context.Messages
                .Where(m => m.ConversationId == conversationId && !m.IsDeleted)
                .OrderByDescending(m => m.DateSentUTC)
                .Include(m => m.SeenBy)
                .Include(m => m.Reactions)
                    .ThenInclude(r => r.Reaction);

            var messages = await baseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();

            return messages;
        }

        public async Task RemoveReactionFromMessageAsync(Guid messageId, Guid userId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var reaction = await context.MessageReactions
                .FirstOrDefaultAsync(r => r.MessageId == messageId && r.UserReactedId == userId);

            var message = await context.Messages
                .Include(m => m.Reactions)
                .FirstOrDefaultAsync(m => m.MessageId == messageId);
            var conversation = await _conversationRepository.GetConversationAsync(message.ConversationId);

            if (reaction != null)
            {
                context.MessageReactions.Remove(reaction);
                await context.SaveChangesAsync();
            }
            
            foreach (var participant in conversation.ConversationParticipants)
            {
                if (participant.UserId != userId)
                    await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "ReactionRemoved", true);
            }
        }

        public async Task<Guid> SendMessageAsync(Message message)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            context.Messages.Add(message);
            await context.SaveChangesAsync();

            var conversation = await _conversationRepository.GetConversationAsync(message.ConversationId);
            foreach (var participant in conversation.ConversationParticipants)
            {
                if (participant.UserId != message.SenderId)
                    await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "MessageReceived", message);
            }

            return message.MessageId;
        }

        public async Task MarkMessageAsDeliveredAsync(Guid messageId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var message = await context.Messages.FindAsync(messageId);
            if (message == null) return;

            message.IsDelivered = true;
            await context.SaveChangesAsync();

            var conversation = await _conversationRepository.GetConversationAsync(message.ConversationId);
            foreach (var participant in conversation.ConversationParticipants)
            {
                if (participant.UserId != message.SenderId)
                    await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "MessageDelivered", true);
            }
        }

        public async Task MarkNewMessagesAsSeenAsync(Guid conversationId, Guid userId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var unseenMessages = await context.Messages
                .Where(m => m.ConversationId == conversationId)
                .Where(m => !m.SeenBy.Any(ms => ms.UserId == userId))
                .ToListAsync();

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
                    await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "MessagesSeen", seenEntries);
            }
        }

    }
}
