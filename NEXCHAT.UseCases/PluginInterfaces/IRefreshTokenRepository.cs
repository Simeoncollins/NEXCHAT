using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEXCHAT.UseCases.PluginInterfaces
{
    public interface IRefreshTokenRepository
    {
        Task SaveAsync(Guid userId, string refreshToken, DateTime expiresAt);
        Task<bool> ValidateAsync(Guid userId, string refreshToken);
        Task RotateAsync(Guid userId, string oldToken, string newToken, DateTime newExpiresAt);
        Task RevokeAsync(Guid userId, string refreshToken);
    }
}
