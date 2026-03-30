using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using NEXCHAT.CoreBusiness;
using NEXCHAT.CoreBusiness.Classes;

namespace NEXCHAT.Client.Services
{
    public class ChatSignalRService
    {
        private HubConnection? _hubConnection;
        private readonly NavigationManager _navigation;

        public event Action<Message>? OnMessageReceived;
        public event Action<bool>? OnMessageDeleted;
        public event Action<bool>? OnMessageEdited;
        public event Action<Guid>? OnMessageDelivered;
        public event Action<MessageSeen>? OnMessageSeen;
        public event Action<ReactionEvent>? OnReactionReceived;
        public event Action<bool>? OnReactionRemoved;
        public event Action<UserTyping>? OnTypingUsers;
        public event Action<Notification>? OnNotificationRecieved;
        public event Action<Conversation>? OnConversationStarted;
        public event Action<UserStatus>? OnUserStatusChanged;

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        public ChatSignalRService(NavigationManager navigation)
        {
            _navigation = navigation;
        }
        public async Task ConnectAsync(Guid userId)
        {
            if (_hubConnection is { State: HubConnectionState.Connected or HubConnectionState.Connecting })
                return;

            var hubUri = _navigation.ToAbsoluteUri($"/chatHub");
            _hubConnection = new HubConnectionBuilder()
              .WithUrl(hubUri)
              .WithAutomaticReconnect()
              .Build();


            RegisterEventHandlers();

            await _hubConnection.StartAsync();
        }

        private void RegisterEventHandlers()
        {
            _hubConnection.On<Conversation>("ConversationStarted", conversation =>
            {
                OnConversationStarted?.Invoke(conversation);
            });
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

            _hubConnection.On<Guid>("MessageDelivered", conversationId =>
            {
                OnMessageDelivered?.Invoke(conversationId);
            });

            _hubConnection.On<MessageSeen>("MessagesSeen", seen =>
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

            _hubConnection.On<UserTyping>("TypingUpdate", userTyping =>
            {
                OnTypingUsers?.Invoke(userTyping);
            });

            _hubConnection.On<Notification>("NotificationRecieved", notification =>
            {
                OnNotificationRecieved?.Invoke(notification);
            });

            _hubConnection.On<UserStatus>("UserStatusChanged", userStatus =>
            {
                OnUserStatusChanged?.Invoke(userStatus);
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
