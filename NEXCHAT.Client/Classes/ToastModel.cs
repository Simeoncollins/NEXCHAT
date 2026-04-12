namespace NEXCHAT.Client.Classes
{
    public class ToastModel
    {
        public Guid Id { get; set; }
        public ToastType Type { get; set; }
        public string Heading { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Url { get; set; }
        public int Timeout { get; set; } = 5000;
    }
}
