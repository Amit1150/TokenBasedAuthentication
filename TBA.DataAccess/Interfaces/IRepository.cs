using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBA.DataAccess.Interfaces
{
    public interface IRepository<TEntity>
       where TEntity : new()
    {
        /// <summary>
        /// This method will execute SP
        /// </summary>
        /// <param name="spName">Name of stored procedures</param>
        /// <param name="parameters">Parameters for stored procedures</param>
        /// <returns></returns>
        IEnumerable<TSEntity> ExecuteSP<TSEntity>(string spName, object parameters = null);
    }
}
