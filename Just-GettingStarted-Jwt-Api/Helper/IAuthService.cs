using Just_GettingStarted_Jwt_Api.Models;

namespace Just_GettingStarted_Jwt_Api.Helper
{
    public interface IAuthService
    {
        Task<bool> UserExists(string email);
        Task<bool> AddUser(User user);
        Task<User?> GetUser(string email);
        Task UpsertRefreshToken(string userEmail, string refreshToken);
        Task DeleteRefreshToken(string userEmail);
    }
}
