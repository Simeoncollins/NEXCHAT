using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace NEXCHAT.Plugin.EFCore
{
    public class RoleRepositoryEfCore : IRoleStore<IdentityRole>
    {
        private readonly IDbContextFactory<NEXCHATDBContext> _dbContextFactory;

        public RoleRepositoryEfCore(IDbContextFactory<NEXCHATDBContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            context.Roles.Add(role);
            await context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            context.Roles.Remove(role);
            await context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public async Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Roles.FindAsync(new object[] { roleId }, cancellationToken);
        }

        public async Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            return await context.Roles
                .FirstOrDefaultAsync(r => r.NormalizedName == normalizedRoleName, cancellationToken);
        }

        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }

        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }

        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }

        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            await using var context = await _dbContextFactory.CreateDbContextAsync();
            context.Roles.Update(role);
            await context.SaveChangesAsync();
            return IdentityResult.Success;
        }

        public void Dispose()
        {
            // DbContext is disposed via the using statements
        }
    }
}
