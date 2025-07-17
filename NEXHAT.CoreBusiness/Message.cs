using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEXCHAT.CoreBusiness
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid SenderId { get; set; }
        public User? Sender { get; set; }
        public Guid ConversationId { get; set; }
        public Conversation? Conversation { get; set; }
        public List<MessageReaction> Reactions { get; set; } = new List<MessageReaction>();
        public bool IsEdited { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public bool IsDelivered { get; set; } = false;
        public DateTime DateSentUTC { get; set; }
        public DateTime ModifiedAt { get; set; }

        // eager loading here
        public List<MessageSeen> SeenBy { get; set; } = new List<MessageSeen>();

        public bool IsUserSeenMessage(Guid userId)
        {
            return SeenBy.Any(ms => ms.UserId == userId);
        }

    }
}
