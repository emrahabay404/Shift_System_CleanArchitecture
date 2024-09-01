using Microsoft.Data.SqlClient;
using System.Data;

namespace Shift_System.Persistence.Contexts
{
    public class DapperContext
    {
        public DapperContext()
        {

        }
        public static string MyConnect { get; set; }
        public static IDbConnection CreateConnection() => new SqlConnection(MyConnect);
    }
}
