using Just_GettingStarted_Jwt_Api.Models;

namespace Just_GettingStarted_Jwt_Api.Helper
{
    public interface ITokenHelper
    {
        TheJwtTokens GenerateTokens(string userEmail);
        string GenerateAccessToken(string userEmail);

        (bool IsValid, string? Email) VerifyAccessToken(string token);
        (bool IsValid, string? Email) VerifyRefreshToken(string token);
    }
}
