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
        [BsonElement("author")]
        public AuthorDocument Author { get; set; } = new();
        [BsonElement("tags")]
        public List<string> Tags { get; set; } = [];
        [BsonElement("viewCount")]
        public int ViewCount { get; set; }
        [BsonElement("isPublished")]
        public bool IsPublished { get; set; }
        [BsonElement("createdAtUtc")]
        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
        [BsonElement("publishedAtUtc")]
        public DateTime? PublishedAtUtc { get; set; }
    }
}
