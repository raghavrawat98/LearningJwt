namespace Just_GettingStarted_Jwt_Api.Models
{
    public class LogoutUser
    {
        public string email { get; set; }
        public string refreshToken { get; set; } // can be password or refresh token, else other people can logout this user just by knowing email
    }
}
