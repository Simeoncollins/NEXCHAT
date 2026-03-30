using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using NEXCHAT.CoreBusiness;
using Shared.DTOS;
using NEXCHAT.UseCases.PluginInterfaces;
using NEXCHAT.CoreBusiness.Enums;
using NEXCHAT.UseCases.Users.Interfaces;

namespace NEXCHAT.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserPasswordStore<User> _userStore;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IUpdateUserStatusUseCase _updateUserStatusUseCase;

        public AuthController(
          IUserPasswordStore<User> userStore,
          UserManager<User> userManager,
          IConfiguration config,
          IUpdateUserStatusUseCase updateUserStatusUseCase)
        {
            _userStore = userStore;
            _userManager = userManager;
            _config = config;
            _updateUserStatusUseCase = updateUserStatusUseCase;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized();

            var claims = new[] {
              new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
              new Claim(ClaimTypes.Name, user.UserName)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            await _updateUserStatusUseCase.ExecuteAsync(user.UserId, StatusType.Online);
            return Ok(new { UserId = user.UserId, Username = user.UserName });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                UserName = dto.Username,
                Email = dto.Email,
                SecurityQuestion = dto.SecurityQuestion,
                SecurityAnswer = dto.SecurityAnswer,
                DateJoined = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return BadRequest(new { Errors = errors });
            }

            return Ok(new { Message = "User registered successfully." });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid parsedId))
            {
                await _updateUserStatusUseCase.ExecuteAsync(parsedId, StatusType.Offline);
            }
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return Ok(new { UserId = userId, Username = User.Identity.Name });
            }
            return Unauthorized();
        }
    }
}
