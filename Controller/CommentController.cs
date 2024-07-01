using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller
{
    [Route("api/comment")]
    [ApiController]

    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IStockRepository _stockRepository;
        public CommentController(ICommentRepository commentRepo, IStockRepository stockRepository)
        {
            _commentRepository = commentRepo;
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var comments = await _commentRepository.GetAllAsync();
            var commentDto = comments.Select(s => s.ToCommentDto());
            return Ok(commentDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if(comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpPost("{Stockid:int}")]
        public async Task<IActionResult> Create([FromRoute] int Stockid, CreateCommentDto commentDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _stockRepository.StockExist(Stockid))
            {
                return BadRequest("Stock does not exist");
            }

            var commentModel = commentDto.ToCommentFromCreate(Stockid);
            await _commentRepository.CreateAsync(commentModel);
            return CreatedAtAction(nameof(GetById),new{ id = commentModel.Id },commentModel.ToCommentDto());
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //cause updateAsync which is excpecting a comment model so we can't send DTO , so we need a maaper for this
            var comment = await _commentRepository.UpdateAsync(id, updateDto.ToCommentFromUpdate());
            if(comment == null)
            {
                return NotFound("Comment not Found");
            }
            return Ok(new
            {
                message = "Stock Updated Successfully",
                UpdatedStock = comment.ToCommentDto()
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var comment = await _commentRepository.DeleteAsync(id);
            if (comment == null) {
                return NotFound("Comment does not exist");
                    }
            return Ok(new
            {
                message = "Comment deleted successfully"
            });
        }
    }
}
