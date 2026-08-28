using System.ComponentModel.DataAnnotations;

namespace YourProjectName.ViewModels
{
    public sealed class MongoBlogPostCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [StringLength(2000, MinimumLength = 10)]
        public string Content { get; set; } = string.Empty;
        [Required]
        public string AuthorName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string AuthorEmail { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = [];
        public bool IsPublished { get; set; }
    }

}
