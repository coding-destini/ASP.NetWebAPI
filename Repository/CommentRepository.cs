using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _dbContest;
        public CommentRepository(ApplicationDBContext dbcontext)
        {
            _dbContest = dbcontext;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _dbContest.Comment.AddAsync(commentModel);
            await _dbContest.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var comment = await _dbContest.Comment.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null) { 
            return null;
            }
            _dbContest.Comment.Remove(comment);
            await _dbContest.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _dbContest.Comment.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _dbContest.Comment.FindAsync(id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existingComment = await _dbContest.Comment.FindAsync(id);
            if(existingComment == null)
            {
                return null;
            }
            existingComment.Title = commentModel.Title;
            existingComment.Content = commentModel.Content;

            await _dbContest.SaveChangesAsync();
            return existingComment;
        }
    }
}
