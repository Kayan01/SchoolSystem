using Microsoft.Extensions.Caching.Memory;
using System;

namespace Shared.Caching
{
    public partial class MemoryCacheManager : ICacheManager
    {
        public IMemoryCache Cache { get; private set; }

        public MemoryCacheManager()
        {
            Cache = new MemoryCache(new MemoryCacheOptions());
        }

        public virtual T Get<T>(string key)
        {
            return (T)Cache.Get(key);
        }

        public virtual void Set(string key, object data, int cacheTime)
        {
            if (data == null)
                return;

            Cache.Set(key, data, DateTime.Now + TimeSpan.FromMinutes(cacheTime));
        }

        public virtual bool IsSet(string key)
        {
            return (Cache.Get(key) != null);
        }

        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }

        //public virtual void RemoveByPattern(string pattern)
        //{
        //    this.RemoveByPattern(pattern, Cache.Select(p => p.Key));
        //}

        //public virtual void Clear()
        //{
        //    Cache..CreateEntry(new object())..Dispose();
        //    foreach (var item in Cache)
        //        Remove(item.Key);
        //}

        public virtual void Dispose()
        {
        }
    }
}