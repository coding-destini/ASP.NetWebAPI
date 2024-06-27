using TestProject.Data;
using TestProject.Interfaces;
using TestProject.Models;

namespace TestProject.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext _dbContext;
        public UserRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> CreateAsync(User userModel)
        {
            await _dbContext.Users.AddAsync(userModel);
            await _dbContext.SaveChangesAsync();
            return userModel;
        }
    }
}
