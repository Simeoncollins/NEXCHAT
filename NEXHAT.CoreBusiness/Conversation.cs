using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEXCHAT.CoreBusiness
{
    public class Conversation
    {
        public Guid ConversationId { get; set; }
        public Guid CreatorId { get; set; }
        public User? Creator { get; set; }
        public List<ConversationParticipant> ConversationParticipants { get; set; } = new List<ConversationParticipant>();
        public List<Message> Messages { get; set; } = new List<Message>();
        public DateTime DateStartedUTC { get; set; }
        public List<ConversationTypingUser> ParticipantsTyping { get; set; } = new List<ConversationTypingUser>();
        public bool IsGroupConversation { get; set; } = false;
        public string GroupName { get; set; } = string.Empty;
        public string GroupCoverPhotoPath { get; set; } = string.Empty;

        // not mapped to database. for ui purposes only
        [NotMapped]
        public int UnreadMessagesCount { get; set; }
        [NotMapped]
        public Message? LastMessage { get; set; }

    }
}
