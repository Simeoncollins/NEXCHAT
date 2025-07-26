namespace NEXCHAT.UseCases.FriendManagement.Interfaces
{
    public interface ICheckIfBlockedByFriendUseCase
    {
        Task<bool> ExecuteAsync(Guid userId, Guid friendId);
    }
}