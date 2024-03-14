namespace EHRApplication.Connection
{
    public class ConnectionString : IConnectionString
    {
        private readonly IConfiguration _configuration;

        public ConnectionString(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetConnectionString()
        {
            return _configuration["ConnectionStrings:DefaultConnection"];
        }
    }
}
