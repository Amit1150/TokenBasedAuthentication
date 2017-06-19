using System.Data;
using TBA.DataAccess.Interfaces;
using TBA.Model;
using Dapper;
using System.Linq;
using TBA.EntityModel;
using System.Collections.Generic;

namespace TBA.DataAccess.Providers
{
    public class AppUserRepository : BaseRepository<AppUser>, IAppUserRepository
    {
        public AppUser GetUserById(int userId)
        {
            using (IDbConnection _connection = DapperConnection)
            {
                var query = "Select * from AppUser where UserId=@UserId";
                return _connection.Query<AppUser>(query, new { UserId = userId }).FirstOrDefault();
            }
        }

        public AppUser GetUserByUserName(string userName)
        {
            using (IDbConnection _connection = DapperConnection)
            {
                var query = "Select * from AppUser where userName=@userName";
                return _connection.Query<AppUser>(query, new { userName = userName }).FirstOrDefault();
            }
        }

        public List<UserRoles> GetUserRoles(int userId)
        {
            using (IDbConnection _connection = DapperConnection)
            {
                var getRoleQuery = "select RoleTypeId,RT.name as RoleName from AppUserRole UR JOIN RoleType RT on RT.Id=UR.RoleTypeId where UR.AppUserId=@userId";
                return _connection.Query<UserRoles>(getRoleQuery, new { userId = userId }).ToList();
            }
        }

        public bool AddUser(AppUser user)
        {
            using(IDbConnection _connection = DapperConnection)
            {
                var query = "INSERT INTO [dbo].[AppUser]([UserName] ,[Name],[IsBlocked] ,[Email] ,[PasswordHash] ,[ImageName]) Values (@UserName,@Name,@IsBlocked,@Email,@PasswordHash,@ImageName)";
                return _connection.Execute(query, new
                {
                    UserName = user.UserName,
                    Name = user.Name,
                    IsBlocked = user.IsBlocked,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    ImageName = user.ImageName
                }) > 0;
            }
        }
    }
}
