using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness.Enums;

namespace NEXCHAT.CoreBusiness
{
    public class UserFriend
    {
        public Guid RequesterId { get; set; }
        public User? Requester { get; set; }
        public Guid ReceiverId { get; set; }
        public User? Receiver { get; set; }
        public FriendRequestStatus Status { get; set; } = FriendRequestStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
