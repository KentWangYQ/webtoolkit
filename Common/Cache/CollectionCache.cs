using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Caching;

namespace Common
{
    public class CollectionCache
    {
        private static bool enabled = System.Configuration.ConfigurationManager.AppSettings["DataCacheEnabled"].ToLower().Equals("true");
        private static readonly object CacheLocker = new object();
        private static DateTime CleanTime = DateTime.UtcNow;
        private static int CleanInterval = 1800;           //30 Mins

        public static IEnumerable<object> Get(string cacheKey, string valueKey)
        {
            var collection = DataCache.Get(cacheKey) as IEnumerable<CacheModel>;
            if (collection == null)
                return null;

            var result = collection.Where(c => c.Key == valueKey && !c.IsExpired);
            if (result == null || result.Count() <= 0)
                return null;

            return result.Select(v => v.Value);
        }
        public static void InsertDefault(string cacheKey, string valueKey, Object value)
        {
            if (enabled)
                Insert(cacheKey, valueKey, value, TimeSpan.FromSeconds(CommonConstants.DataCacheDuration));
        }
        public static void Insert(string cacheKey, string valueKey, Object value, DateTime absoluteExpiration)
        {
            if (enabled)
                Insert(cacheKey, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, valueKey, value, absoluteExpiration, Cache.NoSlidingExpiration);
        }
        public static void Insert(string cacheKey, string valueKey, Object value, TimeSpan slidingExpiration)
        {
            if (enabled)
                Insert(cacheKey, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, valueKey, value, Cache.NoAbsoluteExpiration, slidingExpiration);
        }


        public static void Insert(string cacheKey, string valueKey, Object value, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            if (enabled)
                Insert(cacheKey, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, valueKey, value, absoluteExpiration, slidingExpiration);
        }

        public static void Insert(string cacheKey, DateTime collectionAbsoluteExpiration, TimeSpan collectionSlidingExpiration, string valueKey, Object value, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            if (enabled)
            {
                var data = DataCache.Get(cacheKey) as IList<CacheModel>;
                if (data == null)
                {
                    data = new List<CacheModel>();
                    DataCache.Insert(cacheKey, data, collectionAbsoluteExpiration, collectionSlidingExpiration);
                }

                lock (CacheLocker)
                {
                    data.Add(new CacheModel() { Key = valueKey, Value = value, InsertTime = DateTime.UtcNow, AbsoluteExpiration = absoluteExpiration.ToUniversalTime(), SlidingExpiration = slidingExpiration });
                }

                //Periodic Cleanup
                if (CleanTime.AddSeconds(CleanInterval) <= DateTime.Now)
                {
                    new Thread(Clean).Start();
                }
            }
        }

        public static void Remove(string cacheKey, string valueKey)
        {
            var data = DataCache.Get(cacheKey) as IEnumerable<CacheModel>;
            if (data != null)
            {
                lock (CacheLocker)
                {
                    data = data.Where(e => e.Key != valueKey);
                }
            }
        }

        private static void Clean()
        {
            lock (CacheLocker)
            {
                var cache = DataCache.GetEnumerator();
                while (cache.MoveNext())
                {
                    if (cache.Value is IEnumerable<CacheModel>)
                    {
                        var collection = cache.Value as IEnumerable<CacheModel>;
                        collection = collection.Where(e => !e.IsExpired);
                    }
                }
            }
        }
    }
}