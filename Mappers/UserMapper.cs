using TestProject.Dtos;
using TestProject.Models;

namespace TestProject.Mappers
{
    public static class UserMapper
    {
        public static User ToCreateFromUserDto(this CreateUserDto Userdto)
        {
            return new User
            {
                Name = Userdto.Name,
                Email = Userdto.Email,
                UserName = Userdto.UserName
            };
        }

        public static UserDto ToUserDto(this User user) {
            return new UserDto {
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName
            };
        }
    }
}
