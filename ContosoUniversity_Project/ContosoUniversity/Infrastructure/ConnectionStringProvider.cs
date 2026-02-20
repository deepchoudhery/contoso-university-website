namespace ContosoUniversity
{
    public static class ConnectionStringProvider
    {
        public static string EfConnectionString { get; set; } =
            "Data Source=.\\SQLEXPRESS;Initial Catalog=ContosoUniversity;Integrated Security=True;MultipleActiveResultSets=True;Encrypt=false;TrustServerCertificate=true";

        public static string SqlConnectionString { get; set; } =
            "Data Source=.\\SQLEXPRESS;Initial Catalog=ContosoUniversity;Integrated Security=True;Encrypt=false;TrustServerCertificate=true";
    }
}
