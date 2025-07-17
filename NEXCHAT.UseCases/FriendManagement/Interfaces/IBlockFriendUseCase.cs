namespace NEXCHAT.UseCases.FriendManagement.Interfaces
{
    public interface IBlockFriendUseCase
    {
        Task ExecuteAsync(Guid userId, Guid friendId);
    }
}