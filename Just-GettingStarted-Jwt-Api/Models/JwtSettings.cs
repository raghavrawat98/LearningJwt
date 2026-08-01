namespace Just_GettingStarted_Jwt_Api.Models
{
    public class JwtSettings
    {
        public string AccessTokenSecret { get; set; }
        public int AccessTokenDuration { get; set; } // in minutes
        public string RefreshTokenSecret { get; set; }
        public int RefreshTokenDuration { get; set; } // in hrs
    }
}
