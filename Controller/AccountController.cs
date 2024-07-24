using api.Dtos.Account;
using api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace api.Controller
{
    [Route("/api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        public AccountController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }


        [HttpPost("register")]
        // Registers a new user with the provided registration details
        public async Task<IActionResult> Register([FromBody] RegisterDto registerdto)
        {
            try
            {
                // Check if the model state is valid
                if (!ModelState.IsValid)
                {
                    // Return a bad request response if the model state is invalid
                    return BadRequest(ModelState);
                }

                // Create a new AppUser object with the details from the registration DTO
                var appUser = new AppUser
                {
                    UserName = registerdto.UserName,
                    Email = registerdto.EmailAddress
                };

                // Create the user asynchronously with the provided password
                var createdUser = await _userManager.CreateAsync(appUser, registerdto.Password);

                // Check if the user creation was successful
                if (createdUser.Succeeded)
                {
                    // Add the newly created user to the "User" role
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

                    // Check if adding the user to the role was successful
                    if (roleResult.Succeeded)
                    {
                        // Return an OK response with a message and the created user details
                        return Ok(("User Created", new { User = appUser }));
                    }
                    else
                    {
                        // Return a 500 status code with the errors from adding the role
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    // Return a 500 status code with the errors from user creation
                    return StatusCode(500, createdUser.Errors);
                }
            }
            catch (Exception ex)
            {
                // Return a 500 status code with the exception message if an error occurs
                return StatusCode(500, ex.Message);
            }
        }
    }
}
