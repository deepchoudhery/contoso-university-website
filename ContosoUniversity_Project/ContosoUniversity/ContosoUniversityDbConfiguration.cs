using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;

namespace ContosoUniversity
{
    public class ContosoUniversityDbConfiguration : System.Data.Entity.DbConfiguration
    {
        public ContosoUniversityDbConfiguration()
        {
            SetProviderServices("System.Data.SqlClient", SqlProviderServices.Instance);
            SetDefaultConnectionFactory(new SqlConnectionFactory());
        }
    }
}
