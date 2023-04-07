using System;
using Runtime.Messaging.Events;
using UniRx;

namespace Runtime.Messaging
{
    public class EventSubscriber
    {
        public IObservable<GameFatalError> OnGameFatalError()
        {
            return MessageBroker.Default.Receive<GameFatalError>();
        }

        public IObservable<GamePreloadingInfo> OnGamePreloadingInfo()
        {
            return MessageBroker.Default.Receive<GamePreloadingInfo>();
        }

        public IObservable<GridInstantiated> OnGridInstantiated()
        {
            return MessageBroker.Default.Receive<GridInstantiated>();
        }

        public IObservable<PreloadComplete> OnPreloadComplete()
        {
            return MessageBroker.Default.Receive<PreloadComplete>();
        }
    }
}