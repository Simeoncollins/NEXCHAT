using NEXCHAT.Client.Classes;

namespace NEXCHAT.Client.Services
{
    public interface IToastService
    {
        event Action<ToastModel> OnShow;
        void ShowToast(ToastType type, string heading, string message, string? url = null, int timeout = 10000);
    }
}
