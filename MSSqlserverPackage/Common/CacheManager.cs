using System.Runtime.Caching;

namespace MSSqlserverPackage.Common
{
    public class CacheManager
    {
        public static object GetValue(string key)

        {
            ObjectCache cache = MemoryCache.Default;
            if (cache.Contains(key))
            {
                return cache["Tables"];
            }
            return null;
        }

        public static void SetValue(string key, object value, CacheItemPolicy policy = null)
        {
            if (value == null) return;
            var cache = MemoryCache.Default;
            if (policy == null)
                policy = new CacheItemPolicy();
            cache.Set(key, value, policy);
        }
    }
}
