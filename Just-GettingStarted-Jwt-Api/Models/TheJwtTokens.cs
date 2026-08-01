namespace Just_GettingStarted_Jwt_Api.Models
{
    public class TheJwtTokens
    {
        public TheJwtTokens(
                string accessToken,
                string refreshToken
            )
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
        public string AccessToken { get; }
        public string RefreshToken { get; }
    }
}
