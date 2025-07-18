using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NEXCHAT.CoreBusiness;
using Shared.DTOS;
using NEXCHAT.UseCases.PluginInterfaces;

namespace NEXCHAT.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserPasswordStore<User> _userStore;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IRefreshTokenRepository _refreshRepo;      // to save refresh tokens

        public AuthController(
          IUserPasswordStore<User> userStore,
          UserManager<User> userManager,
          IConfiguration config,
          IRefreshTokenRepository repo)
        {
            _userStore = userStore;
            _userManager = userManager;
            _config = config;
            _refreshRepo = repo;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                return Unauthorized();

            var accessToken = GenerateJwt(user);
            var refreshToken = Guid.NewGuid().ToString();
            await _refreshRepo.SaveAsync(user.UserId, refreshToken, DateTime.UtcNow.AddDays(30));

            return Ok(new TokenResponseDto {UserId = user.UserId, AccessToken = accessToken, RefreshToken = refreshToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto dto)
        {
            // 1) Validate the stored refresh token from your repository:
            var valid = await _refreshRepo.ValidateAsync(dto.UserId, dto.RefreshToken);
            if (!valid) return Unauthorized();

            // 2) Issue a new access token (and optionally a new refresh token):
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            var newAccess = GenerateJwt(user);
            var newRefresh = Guid.NewGuid().ToString();
            await _refreshRepo.RotateAsync(dto.UserId, dto.RefreshToken, newRefresh, DateTime.UtcNow.AddDays(30));

            return Ok(new TokenResponseDto { UserId = user.UserId, AccessToken = newAccess, RefreshToken = newRefresh });
        }

        private string GenerateJwt(User user)
        {
            var creds = new SigningCredentials(
              new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
              SecurityAlgorithms.HmacSha256);

            var claims = new[] {
              new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
              new Claim(ClaimTypes.Name, user.UserName)
            };

            var token = new JwtSecurityToken(
              issuer: _config["Jwt:Issuer"],
              audience: _config["Jwt:Audience"],
              claims: claims,
              expires: DateTime.UtcNow.AddMinutes(15),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
