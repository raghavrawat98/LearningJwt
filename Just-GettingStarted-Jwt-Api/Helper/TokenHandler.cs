using Just_GettingStarted_Jwt_Api.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Just_GettingStarted_Jwt_Api.Helper
{
    public class TokenHandler : ITokenHelper
    {
        private readonly JwtSettings _jwtSettings;
        public TokenHandler(
            IOptions<JwtSettings> jwtSettings
            ) 
        {
            _jwtSettings = jwtSettings.Value;
        }
        TheJwtTokens ITokenHelper.GenerateTokens(string userEmail)
        {
            string accessToken = CreateJwtToken(userEmail, _jwtSettings.AccessTokenSecret, _jwtSettings.AccessTokenDuration); // 15 min 
            string refreshToken = CreateJwtToken(userEmail, _jwtSettings.RefreshTokenSecret, _jwtSettings.RefreshTokenDuration * 60); // 24 hrs = 24 * 60 min
            return new TheJwtTokens(accessToken, refreshToken);
        }

        public string CreateJwtToken(string whatItClaims,string secret, int expiryMinutes)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var token = new JwtSecurityToken(
                claims: new[] { new Claim(JwtRegisteredClaimNames.Jti, whatItClaims) },
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        (bool IsValid, string? Email) ITokenHelper.VerifyAccessToken(string token)
        {
            return VerifyJwtToken(token, _jwtSettings.AccessTokenSecret);
        }

        (bool IsValid, string? Email) ITokenHelper.VerifyRefreshToken(string token)
        {
            return VerifyJwtToken(token, _jwtSettings.RefreshTokenSecret);
        }

        public (bool IsValid, string? Email) VerifyJwtToken(string token, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secret);

            // 1. Define the validation parameters
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                // Set these to true if you set Issuer/Audience in CreateJwtToken
                ValidateIssuer = false,
                ValidateAudience = false,

                // Validate token expiration time
                ValidateLifetime = true,

                // By default, ASP.NET adds a 5-minute buffer to account for clock differences across servers.
                // Set ClockSkew to zero for exact expiration timing.
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                // 2. Validate the token (throws an exception if invalid or expired)
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // 3. Extract your email claim from the Jti claim position
                string? email = principal.FindFirst(JwtRegisteredClaimNames.Jti)?.Value
                                ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                
                if (email == null) { return (false, null); }

                return (true, email);
            }
            catch (SecurityTokenExpiredException)
            {
                // Token has passed its expiry minute mark
                return (false, null);
            }
            catch (Exception)
            {
                // Token is invalid (tampered signature, malformed string, etc.)
                return (false, null);
            }
        }

        public string GenerateAccessToken(string userEmail)
        {
            string accessToken = CreateJwtToken(userEmail, _jwtSettings.AccessTokenSecret, _jwtSettings.AccessTokenDuration); // 15 min 
            return accessToken;
        }
    }
}
