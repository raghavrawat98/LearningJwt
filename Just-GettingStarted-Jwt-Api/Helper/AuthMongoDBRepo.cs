using Just_GettingStarted_Jwt_Api.Controllers;
using Just_GettingStarted_Jwt_Api.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Just_GettingStarted_Jwt_Api.Helper
{
    public class AuthMongoDBRepo : IAuthService
    {
        private IMongoCollection<User> _users;
        private AuthMongoDBSettings _authMongoDBSettings;
        private readonly ILogger<AuthMongoDBRepo> _logger;
        private IMongoDatabase _mongoDatabase;

        public AuthMongoDBRepo(
            IOptions<AuthMongoDBSettings> authMongoDBSettings,
            ILogger<AuthMongoDBRepo> logger
            , IMongoDatabase mongoDatabase
            )
        {
            _logger = logger;
            _authMongoDBSettings = authMongoDBSettings.Value;
            _mongoDatabase = mongoDatabase;
            _users = _mongoDatabase.GetCollection<User>(_authMongoDBSettings.Collection);
        }
        public async Task<bool> UserExists(string email)
        {
            try
            {
                // Check if any document matches the email
                bool exists = await _users.Find(u => u.Email == email).AnyAsync();

                return exists;
            }
            catch (MongoConfigurationException)
            {
                _logger.LogDebug($"Configuration problem");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Something went wrong {ex.Message}");
                return false;
            }
        }

        public async Task<bool> AddUser(User user)
        {
            try
            {
                // Insert object to document
                await _users.InsertOneAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Something went wrong {ex.Message}");
                return false;
            }
        }

        public async Task<User?> GetUser(string email)
        {
            try
            {
                // If the details matches record, then valid is true else false
                User? validUser = await _users.Find
                    (u => u.Email == email).FirstOrDefaultAsync();

                return validUser;
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Something went wrong {ex.Message}");
                return null;
            }
        }

        public async Task UpsertRefreshToken(string userEmail, string refreshToken)
        {
            try
            {
                // Filter will find
                // Update will replace
                var filter = Builders<User>.Filter.Eq(d => d.Email, userEmail);
                var update = Builders<User>.Update.Set(d => d.RefreshToken, refreshToken);
                
                await _users.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Something went wrong {ex.Message}");
            }
        }

        public async Task DeleteRefreshToken(string userEmail)
        {
            try
            {
                // 1. Create a filter to locate the user by email
                var filter = Builders<User>.Filter.Eq(u => u.Email, userEmail);

                // 2.X Define the update to remove/unset the RefreshToken field
                var update = Builders<User>.Update.Unset(u => u.RefreshToken);

                // 2.Y Explicitly SET the RefreshToken to null
                // var update = Builders<User>.Update.Set(u => u.RefreshToken, null);

                // 3. Execute the update
                var result = await _users.UpdateOneAsync(filter, update);

                // Returns true if a matching user was found and updated
                // return result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"Something went wrong {ex.Message}");
            }
        }
    }
}
