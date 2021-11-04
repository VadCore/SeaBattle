﻿using Microsoft.AspNetCore.Identity;
using SeaBattle.Domain.Entities;
using SeaBattle.Domain.Enums;
using SeaBattle.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SeaBattle.Infrastructure.CustomIdentityProvider
{

    public class CustomUserStore : IUserRoleStore<User>,
        IUserPasswordStore<User>
    {
        private readonly IRepository<User> _users;
        private readonly IRoleStore<Role> _roleStore;
        

        public CustomUserStore(IRepository<User> users, IRoleStore<Role> roleStore)
        {
            _users = users;
            _roleStore = roleStore;
        }

        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            var result = _users.Add(user);

            _users.SaveChanges();

            if(result != null)
            {
                return await Task.FromResult(IdentityResult.Success);
            }

            return IdentityResult.Failed(new IdentityError { Description = $"Could not create user {user.Email}." });
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            _users.Delete(user);

            _users.SaveChanges();

            return await Task.FromResult(IdentityResult.Success);
        }

        public Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (userId == null) throw new ArgumentNullException(nameof(userId));

            if (!Int32.TryParse(userId, out int idInt))
            {
                throw new ArgumentException("Not a valid id", nameof(userId));
            }

            var result = _users.GetById(idInt);

            if(result == null)
            {
                throw new ArgumentException("This is not found", nameof(userId));
            }

            return Task.FromResult(result);
        }

        public Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (normalizedUserName == null) 
            {
                throw new ArgumentNullException(nameof(normalizedUserName));
            }

            var user = _users.FindFirst(u => u.NormalizedName == normalizedUserName);

            //if(user == null)
            //{
                //throw new ArgumentNullException(nameof(normalizedUserName));
            //}

            return Task.FromResult(user);
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            var result = _users.FindFirst(u => u.Id == user.Id).NormalizedName;

            if (String.IsNullOrEmpty(result))
            {
                throw new ArgumentException("This user name not found", nameof(user));
            }

            return Task.FromResult(result);
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            var userPasswordHash = _users.FindFirst(u => u.Id == user.Id).Password;

            if (String.IsNullOrEmpty(userPasswordHash))
            {
                throw new ArgumentException("This user not have password hash", nameof(user));
            }

            return Task.FromResult(userPasswordHash);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }


            return Task.FromResult(user.Id.ToString());
        }

        public async Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            if (!string.IsNullOrEmpty(user.Name))
            {
                return user.Name;
            }

            var userName = _users.FindFirst(u=> u.Password == user.Password && u.Email == user.Email)?.Name;

            if (String.IsNullOrEmpty(userName))
            {
                return null;
            }

            return await Task.FromResult(userName);
        }

        public async Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            var isHasPassword = String.IsNullOrWhiteSpace(_users.GetById(user.Id).Password);

            return await Task.FromResult(isHasPassword);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(normalizedName)) throw new ArgumentNullException(nameof(normalizedName));

            user.NormalizedName = normalizedName;
            _users.Update(user);

            _users.SaveChanges();

            return Task.FromResult<object>(null);
        }

        public Task SetPasswordHashAsync(User user, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            user.Password = password;
            _users.Update(user);

            _users.SaveChanges();

            return Task.FromResult<object>(null);

        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (string.IsNullOrWhiteSpace(userName)) throw new ArgumentNullException(nameof(userName));

            user.Name = userName;
            _users.Update(user);

            _users.SaveChanges();

            return Task.FromResult<object>(null);
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            var getUser = _users.GetById(user.Id);

            if(getUser == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = $"Could not update user {user}." });
            }

             _users.Update(user);

            _users.SaveChanges();

            return await Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
           
        }

        public async Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            var role = await _roleStore.FindByNameAsync(roleName, cancellationToken);

            user.RoleId = role.Id;

            _users.Update(user);
            _users.SaveChanges();
        }

        public Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            user.RoleId =  (int)RoleType.Undefined;

            _users.Update(user);
            _users.SaveChanges();

            return Task.FromResult<object>(null);
        }

        public async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            var role = await _roleStore.FindByIdAsync(user.RoleId.ToString(), cancellationToken);

            var roleName = await _roleStore.GetRoleNameAsync(role, cancellationToken);

            return new List<string> { roleName };
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));

            var role = await _roleStore.FindByNameAsync(roleName, cancellationToken);

            return role.Id == user.RoleId;
        }

        public async Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (roleName == null) throw new ArgumentNullException(nameof(roleName));

            var roleId = (await _roleStore.FindByNameAsync(roleName, cancellationToken)).Id;

            return _users.FindAll(u => u.RoleId == roleId).ToList();
        }
    }
}