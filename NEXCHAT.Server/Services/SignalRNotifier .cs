using Microsoft.AspNetCore.SignalR;
using NEXCHAT.Server.Hubs;
using NEXCHAT.CoreBusiness.Interfaces;

namespace NEXCHAT.Server.Services
{
    public class SignalRNotifier : IRealTimeNotifier
    {
        private readonly IHubContext<ChatHub> _hubContext;

        public SignalRNotifier(IHubContext<ChatHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyGroupAsync(string groupName, string method, object data)
        {
            await _hubContext.Clients.Group(groupName).SendAsync(method, data);
        }
    }
}
