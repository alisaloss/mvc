using MongoDB.Bson.Serialization.Attributes;

namespace YourProjectName.Models
{
    public sealed class AuthorDocument
    {
        [BsonElement("authorId")]
        public string AuthorId { get; set; } = string.Empty;
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;
        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;
    }
}