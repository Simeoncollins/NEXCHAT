using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEXCHAT.CoreBusiness
{
    public class ConversationTypingUser
    {
        public Guid ConversationId { get; set; }
        public Conversation? Conversation { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }

        [NotMapped]
        public DateTime LastTypingTime { get; set; }

        [NotMapped]
        public string Name { get; set; } = "";

        [NotMapped]
        public string PhotoPath { get; set; } = "";
    }
}
