using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TBA.DataAccess
{
    public class DataConnection
    {
        public IDbConnection DapperConnection
        {
            get
            {
                return new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ToString());
            }
        }
    }
}
