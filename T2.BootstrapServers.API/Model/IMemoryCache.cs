using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace T2BootstrapServer.API.Model
{
    public interface IMemoryCache : IDisposable
    {
        bool TryGetValue(object key, out object value);
        ICacheEntry CreateEntry(object key);
        void Remove(object key);
    }
}
