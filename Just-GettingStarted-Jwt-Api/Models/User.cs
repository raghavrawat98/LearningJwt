using MongoDB.Bson.Serialization.Attributes;

namespace Just_GettingStarted_Jwt_Api.Models
{
    public class User
    {
        public User(Guid guid)
        {
            UserId = guid.ToString();
        }

        [BsonId]
        public readonly string UserId;
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string? RefreshToken { get; set; }
    }
}
