using TestProject.Models;

namespace TestProject.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User userModel);
    }
}
