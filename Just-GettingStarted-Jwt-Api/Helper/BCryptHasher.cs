namespace Just_GettingStarted_Jwt_Api.Helper
{
    public class BCryptHasher : ICryptHelper
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string userPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, userPassword);
        }
    }
}
