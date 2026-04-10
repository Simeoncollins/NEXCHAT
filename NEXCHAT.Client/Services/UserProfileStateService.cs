using System;

namespace NEXCHAT.Client.Services
{
    public class UserProfileStateService
    {
        public event Action? OnProfileUpdated;

        public void NotifyProfileUpdated()
        {
            OnProfileUpdated?.Invoke();
        }
    }
}
