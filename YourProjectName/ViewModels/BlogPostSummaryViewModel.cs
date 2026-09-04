namespace YourProjectName.ViewModels
{
    public sealed class BlogPostSummaryViewModel
    {
        public string? Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public DateTime? PublishedAtUtc { get; set; }
    }

}
