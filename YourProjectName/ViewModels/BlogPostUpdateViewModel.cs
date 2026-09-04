using System.ComponentModel.DataAnnotations;

namespace YourProjectName.ViewModels
{
    public sealed class BlogPostUpdateViewModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Content { get; set; } = string.Empty;
        [Required]
        public string Category { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = [];
        public bool IsPublished { get; set; }
    }

}
