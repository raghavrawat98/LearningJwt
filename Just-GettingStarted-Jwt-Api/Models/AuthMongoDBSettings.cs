namespace Just_GettingStarted_Jwt_Api.Models
{
    public class AuthMongoDBSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string Database { get; set; } = null!;
        public string Collection { get; set; } = null!;
    }
}
