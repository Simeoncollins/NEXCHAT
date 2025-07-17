namespace NEXCHAT.UseCases.ConversationManagement.Interfaces
{
    public interface IUpdateGroupDetailsUseCase
    {
        Task ExecuteAsync(Guid conversationId, string groupName = "", string groupCoverPhotoPath = "");
    }
}