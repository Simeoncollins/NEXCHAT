using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using NEXCHAT.CoreBusiness.Enums;
using NEXCHAT.UseCases.MessageManagement.Interfaces;
using NEXCHAT.UseCases.Users.Interfaces;

namespace NEXCHAT.Server.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUpdateUserStatusUseCase _updateUserStatusUseCase;
        private readonly IMarkMessageAsDeliveredUseCase _markMessageAsDeliveredUseCase;

        public ChatHub(IUpdateUserStatusUseCase updateUserStatusUseCase, IMarkMessageAsDeliveredUseCase markMessageAsDeliveredUseCase)
        {
            _updateUserStatusUseCase = updateUserStatusUseCase;
            _markMessageAsDeliveredUseCase = markMessageAsDeliveredUseCase;
        }

        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid parsedId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{parsedId}");
                await _updateUserStatusUseCase.ExecuteAsync(parsedId, StatusType.Online);
                await _markMessageAsDeliveredUseCase.ExecuteAsync(parsedId);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId) && Guid.TryParse(userId, out Guid parsedId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{parsedId}");
                await _updateUserStatusUseCase.ExecuteAsync(parsedId, StatusType.Offline);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}
