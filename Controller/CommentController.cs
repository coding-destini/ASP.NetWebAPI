using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller
{
    [Route("api/comment")]
    [ApiController]
    
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepo)
        {
            _commentRepository = commentRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments =  await _commentRepository.GetAllAsync();
            var commentDto = comments.Select(s => s.ToCommentDto());
            return Ok(commentDto);
        }
    }
}
