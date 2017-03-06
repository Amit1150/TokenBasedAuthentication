using Dapper;
using System.Collections.Generic;
using System.Data;
using TBA.DataAccess.Interfaces;

namespace TBA.DataAccess.Providers
{
    public class BaseRepository<TEntity>
        : DataConnection, IRepository<TEntity> where TEntity : new()
    {
        public IEnumerable<TSEntity> ExecuteSP<TSEntity>(string spName, object parameters = null)
        {
            using (IDbConnection _connection = DapperConnection)
            {
                _connection.Open();
                return _connection.Query<TSEntity>(spName, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
