using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEXCHAT.CoreBusiness
{
    public class MessageSeen
    {
        public Guid MessageId { get; set; }
        public Message? Message { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public DateTime SeenAtUTC { get; set; }
    }
}
