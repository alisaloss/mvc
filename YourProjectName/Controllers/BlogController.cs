using Microsoft.AspNetCore.Mvc;
using YourProjectName.Models;

namespace YourProjectName.Controllers
{
    public class BlogController : Controller
    {
        private readonly ILogger<BlogController> _logger;

        public BlogController(ILogger<BlogController> logger)
        {
            _logger = logger;
        }
        private static readonly List<BlogPost> Posts =
[
 new BlogPost
 {
 Id = 1,
 Title = "First Post",
 Content = "This is the first blog post."
 },
 new BlogPost
 {
 Id = 2,
 Title = "Learning MVC",
 Content = "This post is about ASP.NET Core MVC."
 }
];



        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/api/blog")]
        public IActionResult GetAll([FromQuery] string? search)
        {
            _logger.LogInformation(
            "GET request received for all blog posts. Search term: { SearchTerm}", search);
            if (string.IsNullOrWhiteSpace(search))
            {
                return Ok(Posts);
            }
            List<BlogPost> matchingPosts = Posts
            .Where(post =>
            post.Title.Contains(
            search,
            StringComparison.OrdinalIgnoreCase) ||
            post.Content.Contains(
            search,
            StringComparison.OrdinalIgnoreCase))
            .ToList();
            return Ok(matchingPosts);
        }


        [HttpGet("/api/blog/{id:int}")]
        public IActionResult GetById(int id)
        {
            _logger.LogInformation(
            "GET request received for blog post {PostId}.",
            id);
            BlogPost? post = Posts.FirstOrDefault(post => post.Id == id);
            if (post == null)
            {
                _logger.LogWarning(
                "Blog post {PostId} was not found.",
                id);
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost("/api/blog")]
        public IActionResult Create([FromBody] BlogPost newPost)
        {
            _logger.LogInformation(
            "POST request received to create a blog post titled {PostTitle}.",
            newPost.Title);
            if (string.IsNullOrWhiteSpace(newPost.Title))
            {
                _logger.LogWarning("A blog post could not be created because its title was empty.");
            return BadRequest("A title is required.");
            }
            int nextId = Posts.Count == 0
            ? 1
            : Posts.Max(post => post.Id) + 1;
            newPost.Id = nextId;
            Posts.Add(newPost);
            return CreatedAtAction(
            nameof(GetById),
            new { id = newPost.Id },
            newPost);
        }

        [HttpPut("/api/blog/{id:int}")]
        public IActionResult Update(
 int id,
 [FromBody] BlogPost updatedPost)
        {
            _logger.LogInformation(
            "PUT request received for blog post {PostId}.",
            id);
            BlogPost? existingPost =
            Posts.FirstOrDefault(post => post.Id == id);
            if (existingPost == null)
            {
                _logger.LogWarning("Blog post {PostId} could not be updated because it was not found.", id);
                return NotFound();
            }
            if (string.IsNullOrWhiteSpace(updatedPost.Title))
            {
                return BadRequest("A title is required.");
            }
            existingPost.Title = updatedPost.Title;
            existingPost.Content = updatedPost.Content;
            return Ok(existingPost);
        }

        [HttpDelete("/api/blog/{id:int}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation(
            "DELETE request received for blog post {PostId}.",
            id);
            BlogPost? post =
            Posts.FirstOrDefault(post => post.Id == id);
            if (post == null)
            {
                _logger.LogWarning(
                "Blog post {PostId} could not be deleted because it was not found.",          
                id);
                return NotFound();
            }
            Posts.Remove(post);
            return NoContent();
        }

    }
}
