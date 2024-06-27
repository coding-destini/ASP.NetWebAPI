using Microsoft.AspNetCore.Mvc;
using TestProject.Data;
using TestProject.Dtos;
using TestProject.Interfaces;
using TestProject.Mappers;
using TestProject.Models;

namespace TestProject.Controllers
{
    [Route("/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepo)
        {
            _userRepository = userRepo;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userdto)
        {
            var user = userdto.ToCreateFromUserDto();
            await _userRepository.CreateAsync(user);
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
