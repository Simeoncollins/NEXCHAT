namespace NEXCHAT.UseCases.ReactionManagement.Interfaces
{
    public interface ICreateReactionUseCase
    {
        Task<Guid> ExecuteAsync(string reactionName, string emojiPath);
    }
}