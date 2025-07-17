using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NEXCHAT.Server.DTOS;
using NEXCHAT.UseCases.Users;
using NEXCHAT.UseCases.Users.Interfaces;

namespace NEXCHAT.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IGetUserByIdUseCase getUserByIdUseCase;
        private readonly IGetUsersByNameUseCase getUsersByNameUseCase;
        private readonly IUpdateUserStatusUseCase updateUserStatusUseCase;

        public UsersController
            (
            IGetUserByIdUseCase getUserByIdUseCase,
            IGetUsersByNameUseCase getUsersByNameUseCase,
            IUpdateUserStatusUseCase updateUserStatusUseCase
            )
        {
            this.getUserByIdUseCase = getUserByIdUseCase;
            this.getUsersByNameUseCase = getUsersByNameUseCase;
            this.updateUserStatusUseCase = updateUserStatusUseCase;
        }

        // GET: api/users/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(Guid userId)
        {
            var user = await getUserByIdUseCase.ExecuteAsync(userId);
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
