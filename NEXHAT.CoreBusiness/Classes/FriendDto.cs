using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness.Enums;

namespace NEXCHAT.CoreBusiness.Classes
{
    public class FriendDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public StatusType Status { get; set; } = StatusType.Online;
        public string PhotoPath { get; set; } = string.Empty;
        public bool StrangerRequested { get; set; } = false;
        public bool FriendBlocked { get; set; } = false;

        public FriendDto(Guid userId, string userName, string email, StatusType status, string photoPath, bool strangerRequested, bool friendBlocked)
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            Status = status;
            PhotoPath = photoPath;
            StrangerRequested = strangerRequested;
            FriendBlocked = friendBlocked;
        }
    }
}
