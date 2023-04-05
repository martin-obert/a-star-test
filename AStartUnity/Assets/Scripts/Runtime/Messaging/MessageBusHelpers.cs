using System;

namespace Runtime.Messaging
{
    public static class MessageBusHelpers
    {
        public static void AddTo(this IDisposable item, CompositeDisposable disposable)
        {
            disposable.Add(item);
        }
    }
}