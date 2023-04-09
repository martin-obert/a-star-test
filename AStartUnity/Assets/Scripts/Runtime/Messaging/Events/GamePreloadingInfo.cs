namespace Runtime.Messaging.Events
{
    public sealed class GamePreloadingInfo
    {
        public GamePreloadingInfo(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}