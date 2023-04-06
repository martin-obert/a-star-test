using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Runtime.Messaging
{
    public sealed class MessageBus
    {
        private bool _isDisposed;

        public static MessageBus Instance { get; } = new ();

        private readonly IDictionary<Type, IList<Subscription>> _observers = new Dictionary<Type, IList<Subscription>>();

        public IDisposable Subscribe<T>(Action<T> action)
        {
            ThrowIfAlreadyDisposed();
            var type = typeof(T);

            if (!_observers.TryGetValue(type, out var list))
            {
                _observers.Add(type, list = new List<Subscription>());
            }

            var subscription = new Subscription(s => list.Remove(s), v => action((T)v));

            list.Add(subscription);

            return subscription;
        }
        
        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            
            var values = _observers.Values.SelectMany(x=>x).ToArray();
            
            foreach (var value in values)
            {
                value.Dispose();
            }
        }

        public void Publish<T>(T value)
        {
            ThrowIfAlreadyDisposed();
            if (!_observers.TryGetValue(typeof(T), out var observers))
            {
                return;
            }
            var subscriptions = observers.ToArray();
            foreach (var observer in subscriptions)
            {
                observer.OnNext(value);
            }
          
        }
        public async UniTask PublishOnMainThreadAsync<T>(T value)
        {
            ThrowIfAlreadyDisposed();
            await UniTask.SwitchToMainThread();
            Publish(value);
        }


        private sealed class Subscription : IDisposable
        {
            private readonly Action<Subscription> _onDispose;
            private readonly Action<object> _onNext;
            private bool _isDisposed;
            
            public Subscription(Action<Subscription> onDispose, Action<object> onNext)
            {
                _onDispose = onDispose;
                _onNext = onNext;
            }

            public void OnNext(object value)
            {
                if(_isDisposed) return;
                _onNext.Invoke(value);
            }

            public void Dispose()
            {
                if(_isDisposed)
                    return;
                _isDisposed = true;
                _onDispose?.Invoke(this);
            }
        }
        private void ThrowIfAlreadyDisposed()
        {
            if (!_isDisposed) return;
            throw new ObjectDisposedException("MessageBus already disposed");
        }
    }
}