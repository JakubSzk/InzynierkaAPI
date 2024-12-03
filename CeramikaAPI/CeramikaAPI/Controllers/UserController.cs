using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using CeramikaAPI.Models;
using CeramikaAPI.Services;

namespace CeramikaAPI.Controllers
{
    
    public class LoginForm
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class RegisterForm : LoginForm
    {
        public string Email { get; set; } = null!;
    }

    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService userService;

        public UserController()
        {
            userService = new UserService();
        }


        [HttpGet]
        [ProducesResponseType<List<UserModel>>(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(userService.GetUsers());
        }

        [HttpPost("login")]
        [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Login([FromForm] LoginForm loginForm)
        {
            UserModel? user = userService.LogUser(loginForm.Username, loginForm.Password);
            return user == null ? Unauthorized() : Ok(user);
        }

        [HttpPost("register")]
        [ProducesResponseType<UserModel>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult Register([FromForm] RegisterForm registerForm)
        {
            UserModel newUser = new UserModel
            {
                Name = registerForm.Username,
                Password = registerForm.Password,
                Email = registerForm.Email
            };
            UserModel? user = userService.AddUser(newUser);
            return user == null ? Unauthorized() : Ok(user);
        }
    }
    
}
