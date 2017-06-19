using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBA.Model;

namespace TBA.DataAccess.Interfaces
{
    public interface IRefreshTokenRepository
    {
        bool SaveRefreshToken(RefreshToken refreshToken);
        RefreshToken GetRefreshTokenDetail(string refreshTokenId);
    }
}
