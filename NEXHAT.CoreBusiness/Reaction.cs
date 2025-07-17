using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEXCHAT.CoreBusiness
{
    public class Reaction
    {
        public Guid ReactionId { get; set; }
        public string ReactionName { get; set; } = string.Empty;
        public string EmojiPath { get; set; } = string.Empty;
    }
}
