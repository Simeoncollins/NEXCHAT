namespace NEXCHAT.UseCases.FriendManagement.Interfaces
{
    public interface IUnBlockFriendUseCase
    {
        Task ExecuteAsync(Guid userId, Guid friendId);
    }
}