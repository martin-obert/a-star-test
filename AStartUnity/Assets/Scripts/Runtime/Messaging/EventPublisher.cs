using Runtime.Messaging.Events;
using UniRx;

namespace Runtime.Messaging
{
    public class EventPublisher
    {
        public void OnGameFatalError(string message)
        {
            MessageBroker.Default.Publish(new GameFatalError(message));
        }

        public void OnGamePreloadingInfo(string message)
        {
            MessageBroker.Default.Publish(new GamePreloadingInfo(message));
        }

        public void OnGridInstantiated()
        {
            MessageBroker.Default.Publish(new GridInstantiated());
        }

        public void OnPreloadComplete()
        {
            MessageBroker.Default.Publish(new PreloadComplete());
        }
    }
}