using System;

namespace T2BootstrapServer.Application
{
    public interface IMemoryCache : IDisposable
    {
        bool TryGetValue(object key, out object value);
        ICacheEntry CreateEntry(object key);
        void Remove(object key);
    }
}
