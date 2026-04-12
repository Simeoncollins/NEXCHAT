using NEXCHAT.Client.Services;

namespace NEXCHAT.Client.Classes
{
    public class ToastService : IToastService
    {
        public event Action<ToastModel>? OnShow;

        public void ShowToast(ToastType type, string heading, string message, string? url = null, int timeout = 10000)
        {
            var toast = new ToastModel
            {
                Id = Guid.NewGuid(),
                Type = type,
                Heading = heading,
                Message = message,
                Url = url,
                Timeout = timeout
            };
            OnShow?.Invoke(toast);
        }
    }
}
