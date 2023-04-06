namespace Runtime.Messaging.Events
{
    public sealed class GameFatalError
    {
        public GameFatalError(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}