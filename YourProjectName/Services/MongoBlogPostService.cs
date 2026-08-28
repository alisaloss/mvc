using YourProjectName.Configuration;
using YourProjectName.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace YourProjectName.Services
{
    public sealed class MongoBlogPostService
    {
        private readonly IMongoCollection<BlogPostDocument> _posts;
    
    private readonly ILogger<MongoBlogPostService> _logger;
        public MongoBlogPostService(
        IMongoClient mongoClient,
        MongoDbSettings settings,
        ILogger<MongoBlogPostService> logger)
        {
            IMongoDatabase database =
            mongoClient.GetDatabase(settings.DatabaseName);
            _posts =
            database.GetCollection<BlogPostDocument>(
            settings.CollectionName);
            _logger = logger;
        }
        public async Task<List<BlogPostDocument>> GetAllAsync(
        CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
            "Retrieving blog post documents from MongoDB.");
            return await _posts
            .Find(Builders<BlogPostDocument>.Filter.Empty)
            .SortByDescending(post => post.CreatedAtUtc)
            .ToListAsync(cancellationToken);
        }
        public async Task<BlogPostDocument?> GetByIdAsync(
        string id,
        CancellationToken cancellationToken = default)
        {
            if (!ObjectId.TryParse(id, out _))
            {
                return null;
            }
            _logger.LogInformation(
            "Retrieving MongoDB blog post {PostId}.",
            id);
            return await _posts
            .Find(post => post.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        }
        public async Task<BlogPostDocument> InsertAsync(
        BlogPostDocument document,
        CancellationToken cancellationToken = default)
        {
            _logger.LogInformation(
            "Inserting blog post {PostTitle} into MongoDB.",
            document.Title);
            await _posts.InsertOneAsync(
            document,
            cancellationToken: cancellationToken);
            return document;
        }
    }
}
