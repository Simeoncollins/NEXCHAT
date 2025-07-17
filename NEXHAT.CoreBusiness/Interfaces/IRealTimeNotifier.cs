using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NEXCHAT.CoreBusiness.Interfaces
{
    public interface IRealTimeNotifier
    {
        Task NotifyGroupAsync(string groupName, string method, object data);
    }
}
