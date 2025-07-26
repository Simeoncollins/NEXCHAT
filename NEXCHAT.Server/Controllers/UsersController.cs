using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOS;
using NEXCHAT.UseCases.Users;
using NEXCHAT.UseCases.Users.Interfaces;
using Microsoft.AspNetCore.Identity;
using NEXCHAT.CoreBusiness;

namespace NEXCHAT.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IGetUserByIdUseCase getUserByIdUseCase;
        private readonly IGetUsersByNameUseCase getUsersByNameUseCase;
        private readonly IUpdateUserStatusUseCase updateUserStatusUseCase;
        private readonly UserManager<User> userManager;

        public UsersController
            (
            IGetUserByIdUseCase getUserByIdUseCase,
            IGetUsersByNameUseCase getUsersByNameUseCase,
            IUpdateUserStatusUseCase updateUserStatusUseCase,
            UserManager<User> userManager
            )
        {
            this.getUserByIdUseCase = getUserByIdUseCase;
            this.getUsersByNameUseCase = getUsersByNameUseCase;
            this.updateUserStatusUseCase = updateUserStatusUseCase;
            this.userManager = userManager;
        }

        // GET: api/users/{userId}
        [HttpGet("{userId}/detailed")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await getUserByIdUseCase.ExecuteAsync(userId);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
        
        // GET: api/users/{userId}
        [HttpGet("{userId}/basic")]
        public async Task<IActionResult> GetUserBasicInfoById(Guid userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // GET: api/users/search
        [HttpGet("search")]
        public async Task<IActionResult> GetUsersByName([FromBody] GetUsersDto dto)
        {
            var users = await getUsersByNameUseCase.ExecuteAsync(dto.name, dto.pageIndex, dto.pageSize);
            return Ok(users);
        }

        // PUT: api/users/status
        [HttpPut("status")]
        public async Task<IActionResult> UpdateUserStatus([FromBody] UpdateUserStatusDto dto)
        {
            if (dto == null)
                return BadRequest("Invalid request.");

            await updateUserStatusUseCase.ExecuteAsync(dto.UserId, dto.StatusType);
            return NoContent();
        }
    }
}
