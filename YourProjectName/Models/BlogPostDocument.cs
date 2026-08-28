using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace YourProjectName.Models
{
    [BsonIgnoreExtraElements]
    public sealed class BlogPostDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;
        [BsonElement("content")]
        public string Content { get; set; } = string.Empty;
        [BsonElement("createdAtUtc")]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
