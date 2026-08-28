using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using YourProjectName.Models;
using YourProjectName.Services;
using YourProjectName.ViewModels;

namespace YourProjectName.Controllers
{
    [ApiController]
    [Route("api/mongo-blog-posts")]
    public sealed class MongoBlogPostsController : ControllerBase
    {
        private readonly MongoBlogPostService _service;
        public MongoBlogPostsController(
        MongoBlogPostService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<BlogPostDocument>>> GetAll(
        CancellationToken cancellationToken)
        {
            List<BlogPostDocument> posts =
            await _service.GetAllAsync(cancellationToken);
            return Ok(posts);
        }
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<BlogPostDocument>> GetById(
        string id,
        CancellationToken cancellationToken)
        {
            BlogPostDocument? post =
            await _service.GetByIdAsync(
            id,
           cancellationToken);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }
        
        [HttpPost]
        public async Task<ActionResult<BlogPostDocument>> Create(
            [FromBody] MongoBlogPostCreateViewModel viewModel,
            CancellationToken cancellationToken)
        {
            BlogPostDocument document = new()
            {
                Title = viewModel.Title.Trim(),
                Content = viewModel.Content.Trim(),
                Author = new AuthorDocument
                {
                    Name = viewModel.AuthorName.Trim(),
                    Email = viewModel.AuthorEmail.Trim()
                },
                Tags = viewModel.Tags,
                ViewCount = 0,
                IsPublished = viewModel.IsPublished,
                CreatedAtUtc = DateTime.UtcNow,
                PublishedAtUtc =
            viewModel.IsPublished
            ? DateTime.UtcNow
            : null
            };
            await _service.InsertAsync(
            document,
            cancellationToken);
            return CreatedAtAction(
            nameof(GetById),
            new { id = document.Id },
            document);
        }

        [HttpPost("seed")]
        public async Task<IActionResult> Seed(
 CancellationToken cancellationToken)
        {
            List<BlogPostDocument> samplePosts =
            [
            new BlogPostDocument
 {
 Title = "MVC Controllers",
 Content = "Controllers receive requests and select application responses.",
 Author = new AuthorDocument
 {
 Name = "Sam Chen",
 Email = "sam@example.com"
 },
 Tags = ["mvc", "aspnet"],
 ViewCount = 20,
 IsPublished = true,
 CreatedAtUtc = DateTime.UtcNow,
 PublishedAtUtc = DateTime.UtcNow
 },
 new BlogPostDocument
 {
 Title = "MongoDB Documents",
 Content = "MongoDB stores data using flexible BSON documents.",
 Author = new AuthorDocument
 {
 Name = "Sam Chen",
 Email = "sam@example.com"
 },
 Tags = ["mongodb", "nosql"],
 ViewCount = 14,
 IsPublished = true,
 CreatedAtUtc = DateTime.UtcNow,
 PublishedAtUtc = DateTime.UtcNow
 },
 new BlogPostDocument
 {
 Title = "Future Article",
 Content = "This document represents an unpublished draft blog post.",
 Author = new AuthorDocument
 {
 Name = "Taylor Singh",
 Email = "taylor@example.com"
 },
 Tags = ["mongodb", "draft"],
 ViewCount = 0,
 IsPublished = false,
 CreatedAtUtc = DateTime.UtcNow,
 PublishedAtUtc = null
 }
            ];
            await _service.InsertManyAsync(
            samplePosts,
            cancellationToken);
            return Ok(new
            {
                inserted = samplePosts.Count
            });
        }

        [HttpPut("{id:length(24)}/view")]
        public async Task<IActionResult> AddView(
            string id,
            CancellationToken cancellationToken)
        {
            bool updated =
            await _service.IncrementViewCountAsync(
            id,
            cancellationToken);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("publish-by-tag/{tag}")]
        public async Task<IActionResult> PublishByTag( string tag, CancellationToken cancellationToken)
        {
            long modified =
            await _service.PublishByTagAsync(
            tag,
            cancellationToken);
            return Ok(new
            {
                tag,
                modified
            });
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(
            string id,
            CancellationToken cancellationToken)
        {
            bool deleted =
            await _service.DeleteByIdAsync(
            id,
            cancellationToken);
            if (!deleted)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("drafts")]
        public async Task<IActionResult> DeleteDrafts(
            CancellationToken cancellationToken)
        {
            long deleted =
            await _service.DeleteDraftsAsync(
            cancellationToken);
            return Ok(new
            {
                deleted
            });
        }
    }
}
