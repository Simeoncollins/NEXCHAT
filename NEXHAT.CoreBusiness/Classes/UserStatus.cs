using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NEXCHAT.CoreBusiness.Enums;

namespace NEXCHAT.CoreBusiness.Classes
{
    public class UserStatus
    {
        public Guid UserId { get; set; }
        public StatusType Status { get; set; }
    }
}
