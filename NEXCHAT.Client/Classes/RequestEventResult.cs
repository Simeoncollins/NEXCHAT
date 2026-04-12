namespace NEXCHAT.Client.Classes
{
    public class RequestEventResult
    {
        public Guid FriendId { get; }
        public bool IsAccepted { get; }

        public RequestEventResult(Guid friendId, bool isAccepted)
        {
            FriendId = friendId;
            IsAccepted = isAccepted;
        }
    }
}
