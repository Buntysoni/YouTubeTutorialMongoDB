using MongoDB.Bson.Serialization.Attributes;

namespace YouTubeTutorialMongoDB.Models
{
    public class Users
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string? _id { get; set; }
        public int UserId { get; set; }
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        public UserAddress? Address { get; set; }
    }

    public class UserAddress
    {
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
    }
}
