# Gemini Search
NuGet <br>
`System.IdentityModel.Tokens.Jwt` <br>

Program <br>

```csharp
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class TokenGenerator
{
    // Generates a short-lived access token and a long-lived refresh token
    public (string AccessToken, string RefreshToken) GenerateTokens(string username, string accessSecret, string refreshSecret)
    {
        // 15-minute access token, 24-hour refresh token
        var accessToken = CreateJwtToken(accessSecret, 15); 
        var refreshToken = CreateJwtToken(refreshSecret, 1440); 
        return (accessToken, refreshToken);
    }

    private string CreateJwtToken(string secret, int expiryMinutes)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var token = new JwtSecurityToken(
            claims: new[] { new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) },
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

```
[|-Back to Token Problem](Token%20Generation%20problem%20-%20Didn't%20Saw%20this%20comming.md)