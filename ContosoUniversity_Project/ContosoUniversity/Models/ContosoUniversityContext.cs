namespace ContosoUniversity.Models
{
    public partial class ContosoUniversityEntities
    {
        public ContosoUniversityEntities()
            : base(ConnectionStringProvider.EfConnectionString)
        {
        }
    }
}
