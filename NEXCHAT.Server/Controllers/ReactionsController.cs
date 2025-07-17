using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NEXCHAT.UseCases.ReactionManagement.Interfaces;

namespace NEXCHAT.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReactionsController : ControllerBase
    {
        private readonly IGetReactionsUseCase getReactionsUseCase;
        private readonly ICreateReactionUseCase createReactionUseCase;
        private readonly IGetReactionByIdUseCase getReactionByIdUseCase;

        public ReactionsController(IGetReactionsUseCase getReactionsUseCase, ICreateReactionUseCase createReactionUseCase, IGetReactionByIdUseCase getReactionByIdUseCase)
        {
            this.getReactionsUseCase = getReactionsUseCase;
            this.createReactionUseCase = createReactionUseCase;
            this.getReactionByIdUseCase = getReactionByIdUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetReactions()
        {
            var reactions = await getReactionsUseCase.ExecuteAsync();
            return Ok(reactions);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReactionById(Guid id)
        {
            var reaction = await getReactionByIdUseCase.ExecuteAsync(id);
            if (reaction == null) {
                return NotFound($"Reaction with ID {id} not found.");
            }
            return Ok(reaction);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReaction(string reactionName, string emojiPath)
        {
            if (string.IsNullOrEmpty(reactionName) || string.IsNullOrEmpty(emojiPath))
            {
                return BadRequest("Reaction name and emoji path cannot be empty.");
            }
            var reactionId = await createReactionUseCase.ExecuteAsync(reactionName, emojiPath);
            return Ok(reactionId);
        }
    } 
}
