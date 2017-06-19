using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using TBA.DataAccess.Interfaces;
using TBA.DataAccess.Providers;
using TBA.Model;

namespace TBA.API.App_Start
{
    public class UserStore : IUserLoginStore<AppUser, Int32>,
       IUserPasswordStore<AppUser, Int32>,
       IUserStore<AppUser, Int32>
    {
        private IAppUserRepository userRepository;
        public UserStore()
        {
            userRepository = new AppUserRepository();
           
        }

        public Task AddLoginAsync(AppUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(AppUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser> FindByIdAsync(int userId)
        {
            
            AppUser result = userRepository.GetUserById(userId);
            if (result != null)
            {
                return Task.FromResult<AppUser>(result);
            }
            return Task.FromResult<AppUser>(null);
        }

        public Task<AppUser> FindByNameAsync(string userName)
        {
            var result = userRepository.GetUserByUserName(userName);
            if (result != null)
            {
                return Task.FromResult<AppUser>(result);
            }
            return Task.FromResult<AppUser>(null);
        }

        public Task UpdateAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Task<string> GetPasswordHashAsync(AppUser user)
        {
            return Task.FromResult<string>(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(AppUser user)
        {
            return Task.FromResult<bool>(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(AppUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult<Object>(null);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (userRepository != null)
                userRepository = null;
        }
    }
}