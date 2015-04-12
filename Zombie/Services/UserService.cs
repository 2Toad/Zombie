using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Barricade;
using Zombie.Models;
using Claim = Zombie.Models.Claim;

namespace Zombie.Services
{
    public sealed class UserService : DbService, IUserService
    {
        public UserService(SiteContext context) : base(context) { }

        public async Task<User> Get(string username, params Expression<Func<User, object>>[] includes)
        {
            return await includes
                .Aggregate(Context.Users.AsQueryable(), (c, i) => c.Include(i))
                .SingleOrDefaultAsync(x => x.Username == username);
        }

        public async Task<User> GetUserByAccessToken(string accessToken, params Expression<Func<User, object>>[] includes)
        {
            return await includes
                .Aggregate(Context.Users.AsQueryable(), (c, i) => c.Include(i))
                .SingleOrDefaultAsync(x => x.AccessToken == accessToken);
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await Context.Users.AnyAsync(u => u.Username == username);
        }

        public void Register(User user, string password, Claim claim)
        {
            user.PasswordSalt = Guid.NewGuid().ToString("N");
            user.PasswordHash = SecurityContext.GeneratePasswordHash(password, user.PasswordSalt);
            user.ActivationCode = Guid.NewGuid().ToString("N");
            user.Claims = new List<Claim> { claim };

            Context.Users.Add(user);
        }

        public async Task<User> ChangeActivationEmail(string activationCode, string email)
        {
            var user = await Context.Users.SingleOrDefaultAsync(u => u.ActivationCode == activationCode);
            if (user != null)
            {
                user.ActivationCode = Guid.NewGuid().ToString("N");
                user.Email = email;
            }
            return user;
        }

        public async Task<User> RenewActivationCode(string activationCode)
        {
            var user = await Context.Users.SingleOrDefaultAsync(u => u.ActivationCode == activationCode);
            if (user != null) user.ActivationCode = Guid.NewGuid().ToString("N");
            return user;
        }

        public async Task<bool> ActivateAccount(string activationCode)
        {
            var user = await Context.Users.SingleOrDefaultAsync(u => u.ActivationCode == activationCode);
            if (user != null) user.ActivationCode = null;
            return user != null;
        }
    }

    public interface IUserService : IDbService
    {
        Task<User> Get(string username, params Expression<Func<User, object>>[] includes);
        Task<User> GetUserByAccessToken(string accessToken, params Expression<Func<User, object>>[] includes);
        Task<bool> UsernameExists(string username);
        void Register(User user, string password, Claim claim);
        Task<User> ChangeActivationEmail(string activationCode, string email);
        Task<User> RenewActivationCode(string activationCode);
        Task<bool> ActivateAccount(string activationCode);
    }
}
