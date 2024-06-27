using Microsoft.AspNetCore.Mvc;
using TestProject.Data;
using TestProject.Dtos;
using TestProject.Mappers;
using TestProject.Models;

namespace TestProject.Controllers
{
    [Route("/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly ApplicationDBContext _dbContext;
        public UserController(ApplicationDBContext dBContext)
        {
            _dbContext = dBContext;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userdto)
        {
            var user = userdto.ToCreateFromUserDto();
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return Ok(
                new
                {
                    message = "User created successfully",
                    CreatedUser = user.ToUserDto()
                }
                );
        }
    }
}
