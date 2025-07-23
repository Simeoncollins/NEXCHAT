namespace NEXCHAT.UseCases.FriendManagement.Interfaces
{
    public interface IRejectFriendRequestUseCase
    {
        Task ExecuteAsync(Guid requesterId, Guid receiverId);
    }
}