using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEXCHAT.CoreBusiness.Classes
{
    public class ReactionEvent
    {
        public Message message { get; set; } = new Message();
        public Reaction reaction { get; set; } = new Reaction();

        public ReactionEvent(Message message, Reaction reaction)
        {
            this.message = message;
            this.reaction = reaction;
        }
    }
}
