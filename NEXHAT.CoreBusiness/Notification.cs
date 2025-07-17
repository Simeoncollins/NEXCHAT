using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness.Enums;

namespace NEXCHAT.CoreBusiness
{
    public class Notification
    {
        public Guid NotificationId { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime DateSent { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public bool IsSeen { get; set; } = false;
    }
}
