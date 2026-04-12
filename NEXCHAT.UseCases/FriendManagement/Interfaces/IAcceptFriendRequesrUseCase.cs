namespace NEXCHAT.UseCases.FriendManagement.Interfaces
{
    public interface IAcceptFriendRequestUseCase
    {
        Task ExecuteAsync(Guid requesterId, Guid receiverId);
    }
}