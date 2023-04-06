using System;
using Runtime.Messaging.Events;
using UniRx;

namespace Runtime.Messaging
{
    public static class EventSubscriber
    {
        public static IObservable<GameFatalError> OnGameFatalError()
        {
            return MessageBroker.Default.Receive<GameFatalError>();
        }
        public static IObservable<GamePreloadingInfo> OnGamePreloadingInfo()
        {
            return MessageBroker.Default.Receive<GamePreloadingInfo>();
        }
        public static IObservable<GridInstantiated> OnGridInstantiated()
        {
            return MessageBroker.Default.Receive<GridInstantiated>();
        }
        public static IObservable<PreloadComplete> OnPreloadComplete()
        {
            return MessageBroker.Default.Receive<PreloadComplete>();
        }
    }
}