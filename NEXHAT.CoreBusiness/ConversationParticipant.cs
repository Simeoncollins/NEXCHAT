using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NEXCHAT.CoreBusiness
{
    public class ConversationParticipant
    {
        public Guid ConversationId { get; set; }
        public Conversation? Conversation { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
