using System;
using System.Collections.Generic;

namespace Runtime.Messaging
{
    public sealed class CompositeDisposable : IDisposable
    {
        private readonly IList<IDisposable> _items = new List<IDisposable>();

        public void Add(IDisposable item) => _items.Add(item);

        public void Dispose()
        {
            foreach (var item in _items)
            {
                item.Dispose();
            }
        }
    }
}