namespace NEXCHAT.UseCases.FriendManagement.Interfaces
{
    public interface IRejectFriendRequestUseCase
    {
        Task ExecuteAsync(Guid friendRequestId);
    }
}