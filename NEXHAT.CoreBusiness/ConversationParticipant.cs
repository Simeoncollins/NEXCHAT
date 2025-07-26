using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NEXCHAT.CoreBusiness
{
    public class ConversationParticipant
    {
        public Guid ConversationId { get; set; }
        [ForeignKey(nameof(ConversationId))]
        public Conversation? Conversation { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }
    }
}
