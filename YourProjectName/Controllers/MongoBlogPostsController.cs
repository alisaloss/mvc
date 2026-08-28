using YourProjectName.Models;
using YourProjectName.Services;
using YourProjectName.ViewModels;
using Microsoft.AspNetCore.Mvc;

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
        [FromBody] BlogPostCreateViewModel viewModel,
        CancellationToken cancellationToken)
        {
            BlogPostDocument document = new()
            {
                Title = viewModel.Title.Trim(),
                Content = viewModel.Content.Trim(),
                CreatedAtUtc = DateTime.UtcNow
            };
            await _service.InsertAsync(
            document,
            cancellationToken);
            return CreatedAtAction(
            nameof(GetById),
            new { id = document.Id },
            document);
        }
    }
}
