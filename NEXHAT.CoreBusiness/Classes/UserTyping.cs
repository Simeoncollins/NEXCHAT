using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEXCHAT.CoreBusiness.Classes
{
    public class UserTyping
    {
        public Guid UserId { get; set; }
        public string Name { get; set; } = "";
        public string PhotoPath { get; set; } = "";
        public Guid ConversationId { get; set; }
        public bool IsTyping { get; set; }
    }
}
