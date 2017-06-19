using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBA.EntityModel;
using TBA.Model;

namespace TBA.DataAccess.Interfaces
{
    public interface IAppUserRepository :IRepository<AppUser>
    {
        AppUser GetUserById(int userId);
        AppUser GetUserByUserName(string userName);
        List<UserRoles> GetUserRoles(int userId);
        bool AddUser(AppUser user);
    }
}
