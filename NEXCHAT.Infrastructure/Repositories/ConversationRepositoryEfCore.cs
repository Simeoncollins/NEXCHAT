using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NEXCHAT.CoreBusiness;
using NEXCHAT.CoreBusiness.Classes;
using NEXCHAT.CoreBusiness.Interfaces;
using NEXCHAT.Infrastructure.Data;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.Infrastructure.Repositories
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
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var conversation = await context.Conversations
                .Where(c => c.ConversationId == conversationId)
                .AsNoTracking()
                .Select(c => new Conversation
                {
                    ConversationId = c.ConversationId,
                    CreatorId = c.CreatorId,
                    Creator = c.Creator,
                    DateStartedUTC = c.DateStartedUTC,
                    IsGroupConversation = c.IsGroupConversation,
                    GroupName = c.GroupName,
                    GroupCoverPhotoPath = c.GroupCoverPhotoPath,

                    ConversationParticipants = c.ConversationParticipants
                        .Select(cp => new ConversationParticipant
                        {
                            UserId = cp.UserId,
                            ConversationId = cp.ConversationId,
                            // User = cp.User // Optional if you need more info
                        }).ToList(),

                    ParticipantsTyping = c.ParticipantsTyping
                        .Select(pt => new ConversationTypingUser
                        {
                            ConversationId = pt.ConversationId,
                            UserId = pt.UserId,
                            Name = pt.User.UserName,
                            PhotoPath = pt.User.PhotoPath
                        }).ToList()
                })
                .FirstOrDefaultAsync();
            return conversation;
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
            var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == userId);

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
                    await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "TypingUpdate", new UserTyping
                    {
                        ConversationId = conversationId,
                         UserId = userId,
                        Name = user == null? "" : user.UserName,
                        PhotoPath = user == null ? "" : user.PhotoPath,
                        IsTyping = isTyping
                    });
                }
            }
        }

        public async Task<Guid> StartConversationAsync(Conversation conversation)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            await context.Conversations.AddAsync(conversation);
            await context.SaveChangesAsync();
            foreach (var participant in conversation.ConversationParticipants)
            {
                await _notifier.NotifyGroupAsync($"user-{participant.UserId}", "ConversationStarted", conversation);
            }
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
