namespace NEXCHAT.UseCases.FriendManagement.Interfaces
{
    public interface ISendFriendRequestUseCase
    {
        Task ExecuteAsync(Guid requesterId, Guid receiverId);
    }
}