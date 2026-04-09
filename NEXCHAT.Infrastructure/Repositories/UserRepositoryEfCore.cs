using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NEXCHAT.CoreBusiness.Enums;
using NEXCHAT.CoreBusiness;
using NEXCHAT.UseCases.PluginInterfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualBasic;
using NEXCHAT.CoreBusiness.Interfaces;
using NEXCHAT.CoreBusiness.Classes;
using NEXCHAT.Infrastructure.Data;

namespace NEXCHAT.Infrastructure.Repositories
{
    public class UserRepositoryEfCore : IUserStore<User>, IUserPasswordStore<User>, IUserRepository
    {
        private readonly IDbContextFactory<NEXCHATDBContext> _dbContextFactory;
        private readonly IRealTimeNotifier _notifier;

        public UserRepositoryEfCore(IDbContextFactory<NEXCHATDBContext> dbContextFactory, IRealTimeNotifier notifier)
        {
            _dbContextFactory = dbContextFactory;
            _notifier = notifier;
        }


        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            // Check for existing username or email
            var existingUser = await context.Users
                .Where(u => u.UserName == user.UserName || u.Email == user.Email)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingUser != null)
            {
                var errors = new List<IdentityError>();

                if (existingUser.UserName == user.UserName)
                {
                    errors.Add(new IdentityError
                    {
                        Code = "DuplicateUserName",
                        Description = $"Username '{user.UserName}' is already taken."
                    });
                }

                if (existingUser.Email == user.Email)
                {
                    errors.Add(new IdentityError
                    {
                        Code = "DuplicateEmail",
                        Description = $"Email '{user.Email}' is already registered."
                    });
                }

                return IdentityResult.Failed(errors.ToArray());
            }

            // No conflict, proceed
            context.Users.Add(user);
            await context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }


        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            var participations = await context.ConversationParticipants
           .Where(cp => cp.UserId == user.UserId)
           .ToListAsync();
            context.ConversationParticipants.RemoveRange(participations);

            var typingIndicators = await context.ConversationTypingUsers
            .Where(ct => ct.UserId == user.UserId)
            .ToListAsync();
            context.ConversationTypingUsers.RemoveRange(typingIndicators);

            var messagesSeen = await context.MessagesSeen
            .Where(ms => ms.UserId == user.UserId)
            .ToListAsync();
            context.MessagesSeen.RemoveRange(messagesSeen);

            var messages = await context.Messages
            .Where(m => m.SenderId == user.UserId)
            .ToListAsync();

            foreach (var message in messages)
            {
                message.IsDeleted = true;
                message.Content = "User no longer exists";
                message.SenderId = Guid.Empty;
                message.ModifiedAt = DateTime.UtcNow;
            }

            context.Users.Remove(user);
            await context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(userId, out var parsedId)) return null;
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Users.FirstOrDefaultAsync(u => u.UserId == parsedId, cancellationToken);
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Users
                .Include(u => u.SentFriendRequests)
                .Include(u => u.ReceivedFriendRequests)
                .FirstOrDefaultAsync(u => u.Email == normalizedUserName || u.UserName == normalizedUserName);
        }

        public async Task<IEnumerable<User>> GetUsersByNameAsync(string name, int pageIndex, int pageSize)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var query = context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(u =>
                    u.FirstName.Contains(name) ||
                    u.LastName.Contains(name) ||
                    u.UserName.Contains(name));
            }

            return await query.OrderBy(u => u.UserName)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }


        // Implement other interface methods similarly

        public void Dispose()
        {
            // EF Core handles context disposal automatically
        }

        // IUserStore implementation
        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserId.ToString());

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
            => Task.FromResult(user.UserName);

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
            => Task.FromResult(user.Email);

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.Email = normalizedName;
            return Task.CompletedTask;
        }
        public Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        // IUserPasswordStore implementation
        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
            => Task.FromResult(user.PasswordHash);

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
            => Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));

        // Friend management implementation
        public async Task SendFriendRequestAsync(Guid requesterId, Guid receiverId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var request = new UserFriend
            {
                RequesterId = requesterId,
                ReceiverId = receiverId,
                Status = FriendRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await context.UserFriends.AddAsync(request);
            await context.SaveChangesAsync();
        }

        public async Task AcceptFriendRequestAsync(Guid requesterId, Guid receiverId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var request = await context.UserFriends.FindAsync(requesterId, receiverId);
            if (request != null)
            {
                request.Status = FriendRequestStatus.Accepted;
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<FriendDto>> GetFriendListAsync(Guid userId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            return await context.UserFriends
        .Where(uf => (uf.RequesterId == userId || uf.ReceiverId == userId) &&
                     uf.Status == FriendRequestStatus.Accepted || uf.Status == FriendRequestStatus.Blocked)
        .Select(uf => new FriendDto(
            uf.RequesterId == userId ? uf.ReceiverId : uf.RequesterId,
            uf.RequesterId == userId ? uf.Receiver.UserName : uf.Requester.UserName,
            uf.RequesterId == userId ? uf.Receiver.Email : uf.Requester.UserName,
            uf.RequesterId == userId ? uf.Receiver.Status : uf.Requester.Status,
            uf.RequesterId == userId ? uf.Receiver.PhotoPath : uf.Requester.PhotoPath,
            false,
            uf.Status == FriendRequestStatus.Blocked? true : false
        ))
        .ToListAsync();
        }

        public async Task BlockFriendAsync(Guid userId, Guid friendId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            // Check if relationship exists
            var relationship = await context.UserFriends
                .FirstOrDefaultAsync(uf =>
                    (uf.RequesterId == userId && uf.ReceiverId == friendId) ||
                    (uf.RequesterId == friendId && uf.ReceiverId == userId));

            if (relationship != null)
            {
                relationship.Status = FriendRequestStatus.Blocked;
            }

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<FriendDto>> GetBlockedFriendsAsync(Guid userId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            return await context.UserFriends
                .Where(uf => (uf.RequesterId == userId || uf.ReceiverId == userId) &&
                             uf.Status == FriendRequestStatus.Blocked)
                .Select(uf => new FriendDto(
                    uf.RequesterId == userId ? uf.ReceiverId : uf.RequesterId,
                    uf.RequesterId == userId ? uf.Receiver.UserName : uf.Requester.UserName,
                    uf.RequesterId == userId ? uf.Receiver.Email : uf.Requester.UserName,
                    uf.RequesterId == userId ? uf.Receiver.Status : uf.Requester.Status,
                    uf.RequesterId == userId ? uf.Receiver.PhotoPath : uf.Requester.PhotoPath,
                    false, false
                ))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<FriendDto>> GetPendingFriendRequestsAsync(Guid userId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            return await context.UserFriends
                .Where(uf => uf.ReceiverId == userId &&
                             uf.Status == FriendRequestStatus.Pending)
                .Include(uf => uf.Requester)
                .Select(uf => new FriendDto(
                    uf.RequesterId,
                    uf.Requester.UserName,
                    uf.Requester.UserName,
                    uf.Requester.Status,
                    uf.Requester.PhotoPath,
                    false, false
                ))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            return await context.Users
                .Include(u => u.SentFriendRequests)
                .Include(u => u.ReceivedFriendRequests)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task RejectFriendRequestAsync(Guid requesterId, Guid receiverId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            var request = await context.UserFriends.FindAsync(requesterId, receiverId);
            if (request != null)
            {
                context.UserFriends.Remove(request);
                await context.SaveChangesAsync();
            }
        }

        public async Task UnBlockFriendAsync(Guid userId, Guid friendId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();

            // Check if relationship exists
            var relationship = await context.UserFriends
                .FirstOrDefaultAsync(uf =>
                    (uf.RequesterId == userId && uf.ReceiverId == friendId) ||
                    (uf.RequesterId == friendId && uf.ReceiverId == userId));

            if (relationship != null)
            {
                relationship.Status = FriendRequestStatus.Accepted;
            }

            await context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(Guid userId, StatusType statusType)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            var friends = await GetFriendListAsync(userId);
            var user = await context.Users.FindAsync(userId);
            if (user != null)
            {
                user.Status = statusType;
                if (statusType == StatusType.Online)
                {
                    user.LastLogin = DateTime.UtcNow;
                }
                await context.SaveChangesAsync();

                foreach (var friend in friends)
                {
                    await _notifier.NotifyGroupAsync($"user-{friend.UserId}", "UserStatusChanged", new UserStatus{UserId = userId , Status = statusType});
                }
            }
        }

        public async Task<bool> IsBlockedByFriendAsync(Guid userId, Guid friendId)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            var friend = await context.UserFriends.FindAsync(userId, friendId);
            if(friend != null && friend.Status == FriendRequestStatus.Blocked)
            {
                return true;
            }
            return false;
        }
    }
}
