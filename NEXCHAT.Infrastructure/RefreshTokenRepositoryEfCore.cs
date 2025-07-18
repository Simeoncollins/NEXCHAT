using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NEXCHAT.CoreBusiness;
using NEXCHAT.Plugin.EFCore;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.Infrastructure
{
    public class RefreshTokenRepositoryEfCore : IRefreshTokenRepository
    {
        private readonly IDbContextFactory<NEXCHATDBContext> _dbFactory;
        public RefreshTokenRepositoryEfCore(IDbContextFactory<NEXCHATDBContext> dbFactory)
            => _dbFactory = dbFactory;

        public async Task SaveAsync(Guid userId, string refreshToken, DateTime expiresAt)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync();
            ctx.RefreshTokens.Add(new RefreshToken
            {
                UserId = userId,
                Token = refreshToken,
                ExpiresAt = expiresAt
            });
            await ctx.SaveChangesAsync();
        }

        public async Task<bool> ValidateAsync(Guid userId, string refreshToken)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync();
            var entry = await ctx.RefreshTokens.FirstOrDefaultAsync(t =>
                t.UserId == userId &&
                t.Token == refreshToken &&
                !t.IsRevoked &&
                t.ExpiresAt > DateTime.UtcNow);
            return entry != null;
        }

        public async Task RotateAsync(Guid userId, string oldToken, string newToken, DateTime newExpiresAt)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync();
            var entry = await ctx.RefreshTokens.FirstOrDefaultAsync(t =>
                t.UserId == userId &&
                t.Token == oldToken);
            if (entry != null)
            {
                entry.IsRevoked = true;
                ctx.RefreshTokens.Add(new RefreshToken
                {
                    UserId = userId,
                    Token = newToken,
                    ExpiresAt = newExpiresAt
                });
                await ctx.SaveChangesAsync();
            }
        }

        public async Task RevokeAsync(Guid userId, string refreshToken)
        {
            await using var ctx = await _dbFactory.CreateDbContextAsync();
            var entry = await ctx.RefreshTokens.FirstOrDefaultAsync(t =>
                t.UserId == userId && t.Token == refreshToken);
            if (entry != null)
            {
                entry.IsRevoked = true;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
