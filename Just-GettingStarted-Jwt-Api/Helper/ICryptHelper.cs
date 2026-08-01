namespace Just_GettingStarted_Jwt_Api.Helper
{
    public interface ICryptHelper
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string userPassword);
    }
}
