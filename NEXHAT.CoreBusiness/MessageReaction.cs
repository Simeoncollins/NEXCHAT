using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEXCHAT.CoreBusiness
{
    public class MessageReaction
    {
        public Guid MessageReactionId { get; set; }
        public Guid ReactionId { get; set; }
        public Reaction? Reaction { get; set; }
        public Guid UserReactedId { get; set; }
        public User? UserReadcted { get; set; }
        public Guid MessageId { get; set; }
        public Message? Message { get; set; }
    }
}
