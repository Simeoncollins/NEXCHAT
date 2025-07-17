using Microsoft.AspNetCore.SignalR.Client;
using NEXCHAT.CoreBusiness;
using NEXCHAT.CoreBusiness.Classes;

namespace NEXCHAT.Client
{
    public class ChatSignalRService
    {
        private HubConnection? _hubConnection;

        public event Action<Message>? OnMessageReceived;
        public event Action<bool>? OnMessageDeleted;
        public event Action<bool>? OnMessageEdited;
        public event Action<bool>? OnMessageDelivered;
        public event Action<bool>? OnMessageSeen;
        public event Action<ReactionEvent>? OnReactionReceived;
        public event Action<bool>? OnReactionRemoved;
        public event Action<bool>? OnTypingUsers;
        public event Action<Notification>? OnNotificationRecieved;
        public event Action<bool>? OnUserStatusChanged;

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        public async Task ConnectAsync(Guid userId)
        {
            if (_hubConnection is { State: HubConnectionState.Connected or HubConnectionState.Connecting })
                return;

            _hubConnection = new HubConnectionBuilder()
                .WithUrl($"https://localhost:5001/chatHub?userId={userId}")
                .WithAutomaticReconnect()
                .Build();

            RegisterEventHandlers();

            await _hubConnection.StartAsync();
        }

        private void RegisterEventHandlers()
        {
            _hubConnection.On<Message>("MessageReceived", message =>
            {
                OnMessageReceived?.Invoke(message);
            });

            _hubConnection.On<bool>("MessageDeleted", deleted =>
            {
                OnMessageDeleted?.Invoke(deleted);
            });

            _hubConnection.On<bool>("MessageEdited", edited =>
            {
                OnMessageEdited?.Invoke(edited);
            });

            _hubConnection.On<bool>("MessageDelivered", delivered =>
            {
                OnMessageDelivered?.Invoke(delivered);
            });

            _hubConnection.On<bool>("MessagesSeen", seen =>
            {
                OnMessageSeen?.Invoke(seen);
            });

            _hubConnection.On<ReactionEvent>("MessageReacted", reaction =>
            {
                OnReactionReceived?.Invoke(reaction);
            });

            _hubConnection.On<bool>("ReactionRemoved", removed =>
            {
                OnReactionRemoved?.Invoke(removed);
            });

            _hubConnection.On<bool>("TypingUpdate", isTyping =>
            {
                OnTypingUsers?.Invoke(isTyping);
            });

            _hubConnection.On<Notification>("NotificationRecieved", notification =>
            {
                OnNotificationRecieved?.Invoke(notification);
            });

            _hubConnection.On<bool>("UserStatusChanged", changed =>
            {
                OnUserStatusChanged?.Invoke(changed);
            });
        }

        public async Task DisconnectAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.StopAsync();
                await _hubConnection.DisposeAsync();
                _hubConnection = null;
            }
        }

        public async Task ReconnectAsync(Guid userId)
        {
            await DisconnectAsync();
            await ConnectAsync(userId);
        }

    }
}
