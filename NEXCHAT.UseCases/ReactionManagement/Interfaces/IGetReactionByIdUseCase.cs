using NEXCHAT.CoreBusiness;

namespace NEXCHAT.UseCases.ReactionManagement.Interfaces
{
    public interface IGetReactionByIdUseCase
    {
        Task<Reaction> ExecuteAsync(Guid reactionId);
    }
}