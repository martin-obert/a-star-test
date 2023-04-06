using Runtime.Messaging.Events;
using UniRx;

namespace Runtime.Messaging
{
    public static class EventPublisher
    {
        public static void OnGameFatalError(string message)
        {
            MessageBroker.Default.Publish(new GameFatalError(message));
        }

        public static void OnGamePreloadingInfo(string message)
        {
            MessageBroker.Default.Publish(new GamePreloadingInfo(message));
        }

        public static void OnGridInstantiated()
        {
            MessageBroker.Default.Publish(new GridInstantiated());
        }

        public static void OnPreloadComplete()
        {
            MessageBroker.Default.Publish(new PreloadComplete());
        }
    }
}