using Microsoft.AspNetCore.Identity;
using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.CustomIdentityProvider
{
    public class CustomRoleStore : IRoleStore<Role>
    {
        private readonly IRepository<Role> _roles;

        public CustomRoleStore(IRepository<Role> roles)
        {
            _roles = roles;
        }

        public async Task<IdentityResult> CreateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            var result = _roles.Add(role);

            _roles.SaveChanges();

            if (result != null)
            {
                return await Task.FromResult(IdentityResult.Success);
            }

            return IdentityResult.Failed(new IdentityError { Description = $"Could not create role{role}." });
        }

        public async Task<IdentityResult> DeleteAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            _roles.Delete(role);

            _roles.SaveChanges();

            return await Task.FromResult(IdentityResult.Success);
        }

        

        public Task<Role> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (roleId == null) throw new ArgumentNullException(nameof(roleId));

            if (!Int32.TryParse(roleId, out int idInt))
            {
                throw new ArgumentException("Not a valid id", nameof(roleId));
            }

            var result = _roles.GetById(idInt);

            if (result == null)
            {
                throw new ArgumentException("This is not found", nameof(roleId));
            }

            return Task.FromResult(result);
        }

        public Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (normalizedRoleName == null)throw new ArgumentNullException(nameof(normalizedRoleName));

            var role = _roles.FindFirst(r => r.NormalizedName == normalizedRoleName);

            if (role == null)
            {
                throw new ArgumentNullException(nameof(normalizedRoleName));
            }

            return Task.FromResult(role);
        }

        public Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            var result = _roles.GetById(role.Id).Name;

            if (String.IsNullOrEmpty(result))
            {
                throw new ArgumentException("This user name not found", nameof(role));
            }

            return Task.FromResult(result);
        }

        public async Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            var userId = _roles.GetById(role.Id).Id.ToString();

            if (String.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("This role not have password hash", nameof(role));
            }

            return await Task.FromResult(userId);
        }

        public async Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            var userName = _roles.GetById(role.Id).Name;

            if (String.IsNullOrEmpty(userName))
            {
                throw new ArgumentException("This user not have role name", nameof(role));
            }

            return await Task.FromResult(userName);
        }

        public async Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            if (string.IsNullOrWhiteSpace(normalizedName)) throw new ArgumentNullException(nameof(normalizedName));

            role.NormalizedName = normalizedName;

            _roles.Update(role);
          
            _roles.SaveChanges();

            await Task.FromResult<object>(null);
        }

        public async Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken)
        {
            await SetNormalizedRoleNameAsync(role, roleName, cancellationToken);
        }

        public async Task<IdentityResult> UpdateAsync(Role role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));

            var getRole = _roles.GetById(role.Id);

            if (getRole == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Could not update role {role}." });
            }

            _roles.Update(role);

            _roles.SaveChanges();

            return await Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
        }
    }
}
