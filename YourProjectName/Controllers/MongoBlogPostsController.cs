using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
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
                Category = viewModel.Category.Trim(),
                Author = new AuthorDocument
                {
                    AuthorId = viewModel.AuthorId.Trim(),
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

        [HttpPost("seed-query-data")]
        public async Task<IActionResult> SeedQueryData(
 CancellationToken cancellationToken)
        {
            await _service.InsertQueryTestDataAsync(
            300,
            cancellationToken);
            return Ok(new
            {
                inserted = 300
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

        [HttpGet("category/{category}")]
        public async Task<ActionResult<List<BlogPostDocument>>>
             GetByCategory(
             string category,
             [FromQuery] int limit = 10,
             CancellationToken cancellationToken = default)
        {
            limit = Math.Clamp(limit, 1, 100);
            List<BlogPostDocument> posts =
            await _service.GetPublishedByCategoryAsync(
            category,
            limit,
            cancellationToken);
            return Ok(posts);
        }

        [HttpGet("author/{authorId}")]
        public async Task<ActionResult<List<BlogPostDocument>>>
             GetByAuthor(
             string authorId,
             CancellationToken cancellationToken)
        {
            List<BlogPostDocument> posts =
            await _service.GetByAuthorAsync(
            authorId,
            cancellationToken);
            return Ok(posts);
        }

        [HttpGet("popular")]
        public async Task<ActionResult<List<BlogPostDocument>>> GetPopularPosts(
        [FromQuery] int minimumViews = 500,
        CancellationToken cancellationToken = default)
        {
            List<BlogPostDocument> posts =
                await _service.GetPopularPostsAsync(
                    minimumViews,
                    cancellationToken);

            return Ok(posts);
        }

        [HttpGet("category/{category}/summary")]
        public async Task<
         ActionResult<List<BlogPostSummaryViewModel>>>
         GetCategorySummary(
         string category,
         [FromQuery] int limit = 10,
         CancellationToken cancellationToken = default)
        {
            limit = Math.Clamp(limit, 1, 100);
            List<BlogPostSummaryViewModel> posts =
            await _service.GetCategorySummariesAsync(
            category,
            limit,
            cancellationToken);
            return Ok(posts);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(
             string id,
             BlogPostUpdateViewModel viewModel,
             CancellationToken cancellationToken)
        {
            bool updated =
                await _service.UpdateAsync(
                    id,
                    viewModel,
                    cancellationToken);
            if (!updated)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("category/{category}/publish")]
        public async Task<IActionResult> PublishCategory(
            string category,
            CancellationToken cancellationToken)
        {
            long modified =
                await _service.PublishCategoryAsync(
                    category,
                    cancellationToken);

            return Ok(new
            {
                modified
            });
        }

        [HttpDelete("category/{category}/drafts")]
                public async Task<IActionResult> DeleteDraftsByCategory(
             string category,
             CancellationToken cancellationToken)
        {
            long deleted =
                await _service.DeleteDraftsByCategoryAsync(
                    category,
                    cancellationToken);

            return Ok(new
            {
                deleted
            });
        }

        [HttpGet("category/{category}/drafts")]
                public async Task<ActionResult<List<BlogPostDocument>>> GetDraftsByCategory(
            string category,
            CancellationToken cancellationToken)
        {
            List<BlogPostDocument> posts =
                await _service.GetDraftsByCategoryAsync(
                    category,
                    cancellationToken);

            return Ok(posts);
        }

        [HttpPost("indexes")]
        public async Task<IActionResult> CreateIndexes(
 CancellationToken cancellationToken)
        {
            await _service.CreateIndexesAsync(
            cancellationToken);
            return Ok(new
            {
                message = "Indexes created."
            });
        }


    }
}
