using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness.Enums;

namespace NEXCHAT.CoreBusiness
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string SecurityQuestion { get; set; } = string.Empty;
        public string SecurityAnswer { get; set; } = string.Empty;
        public StatusType Status { get; set; } = StatusType.Online;
        public DateTime DateJoined { get; set; } = DateTime.UtcNow;
        public DateTime LastLogin { get; set; } = DateTime.UtcNow;
        public List<string> Roles { get; set; } = new();
        public string PhotoPath { get; set; } = string.Empty;
        public List<Notification> Notifications { get; set; } = new List<Notification>();
        public List<UserFriend> SentFriendRequests { get; set; } = new List<UserFriend>();
        public List<UserFriend> ReceivedFriendRequests { get; set; } = new List<UserFriend>();

        [NotMapped]
        public bool StrangerRequested { get; set; } = false;
        public bool isStranger(Guid userId)
        {
            var sentRequest = SentFriendRequests.FirstOrDefault(fr =>
                fr.ReceiverId == userId 
            );

            if (sentRequest != null)
            {
                if (sentRequest.Status == FriendRequestStatus.Pending)
                {
                    StrangerRequested = true; 
                    return true;
                }
                else
                {
                    return false;
                }

            }

            var receivedRequest = ReceivedFriendRequests.FirstOrDefault(fr =>
                fr.RequesterId == userId
            );

            if (receivedRequest != null)
            {
                return false;               
            }

            StrangerRequested = false;      
            return true;                    
        }
    }
}
