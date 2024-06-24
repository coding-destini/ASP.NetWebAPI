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
        public async Task<List<Comment>> GetAllAsync()
        {
            return await _dbContest.Comment.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            return await _dbContest.Comment.FindAsync(id);
        }
    }
}
