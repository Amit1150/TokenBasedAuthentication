using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBA.DataAccess.Interfaces;
using TBA.Model;
using Dapper;
namespace TBA.DataAccess.Providers
{
    public class RefreshTokenRepository : BaseRepository<RefreshToken>, IRefreshTokenRepository
    {
        public bool SaveRefreshToken(RefreshToken refreshToken)
        {
            using (IDbConnection _connection = DapperConnection)
            {
                var deleteToken = "Delete from  [dbo].[RefreshToken] where deviceid=@deviceId";
                _connection.ExecuteScalar<int>(deleteToken, new
                {
                    DeviceId = refreshToken.DeviceId
                });

                var insertRefreskToken = "INSERT INTO [dbo].[RefreshToken]([RefreshTokenId],[UserId],[DeviceType],[DeviceId],[IssuedUtc],[ExpiresUtc],[ProtectedTicket]) " +
                "values (@RefreshTokenId, @UserId,@DeviceType,@DeviceId,@IssuedUtc, @ExpiresUtc, @ProtectedTicket)";

                    return _connection.Execute(insertRefreskToken, new
                    {
                        RefreshTokenId = refreshToken.RefreshTokenId,
                        UserId = refreshToken.UserId,
                        IssuedUtc = refreshToken.IssuedUtc,
                        ExpiresUtc = refreshToken.ExpiresUtc,
                        ProtectedTicket = refreshToken.ProtectedTicket,
                        DeviceType = refreshToken.DeviceType,
                        DeviceId = refreshToken.DeviceId
                    }) > 0;
            }
        }

        public RefreshToken GetRefreshTokenDetail(string refreshTokenId)
        {
            using (IDbConnection _connection = DapperConnection)
            {
                var query = "Select * from RefreshToken where RefreshTokenId=@RefreshTokenId ";
                return _connection.Query<RefreshToken>(query, new { RefreshTokenId = refreshTokenId }).FirstOrDefault();
            }
        }
    }
}
