using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness.Enums;

namespace Shared.DTOS
{
    public class StartConversationRequestDto
    {
        public Guid CreatorId { get; set; }
        public List<Guid> Participants { get; set; } = new();
        public bool IsGroup { get; set; }
        public string GroupName { get; set; } = "";
    }

    public class UpdateGroupDetailsRequestDto
    {
        public string GroupName { get; set; } = "";
        public string GroupCoverPhotoPath { get; set; } = "";
    }

    public class SendNotificationDto
    {
        public Guid UserId { get; set; }
        public NotificationType Type { get; set; }
        public string Content { get; set; } = string.Empty;
    }

    public class GetUsersDto
    {
        public string name { get; set; } = "";
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }   
    
    public class GetConversationDto
    {
        public Guid conversationId { get; set; }
        public int pageIndex { get; set; }
        public int pageSize { get; set; }
    }

    public class UpdateUserStatusDto
    {
        public Guid UserId { get; set; }
        public StatusType StatusType { get; set; }
    }

    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class RefreshDto
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
    }

    public class TokenResponseDto
    {
        public Guid UserId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }


}
