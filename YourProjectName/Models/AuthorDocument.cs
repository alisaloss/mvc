using MongoDB.Bson.Serialization.Attributes;

namespace YourProjectName.Models
{
    public sealed class AuthorDocument
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;
    }
}