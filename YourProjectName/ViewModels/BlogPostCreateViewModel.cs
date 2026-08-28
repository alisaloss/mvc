using System.ComponentModel.DataAnnotations;

namespace YourProjectName.ViewModels
{
    public class BlogPostCreateViewModel
    {
        [Required(ErrorMessage = "Please enter a title.")]
        [StringLength(
        100,
        ErrorMessage = "The title cannot be longer than 100 characters.")]
 public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Please enter the blog post content.")]
        [StringLength(
        2000,
        MinimumLength = 10,
        ErrorMessage = "The content must be between 10 and 2000 characters.")]
 public string Content { get; set; } = string.Empty;
    }
}
