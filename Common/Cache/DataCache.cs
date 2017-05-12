using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Common
{
    public class DataCache
    {
        static bool enabled = System.Configuration.ConfigurationManager.AppSettings["DataCacheEnabled"].ToLower().Equals("true");
        //static bool enabled = true;

        public static DateTime NoAbsoluteExpiration
        {
            get
            {
                return System.Web.Caching.Cache.NoAbsoluteExpiration;
            }
        }

        public static object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        public static void InsertDefault(string key, Object value)
        {
            if (enabled)
                HttpRuntime.Cache.Insert(key, value, null, DateTime.UtcNow.AddSeconds(CommonConstants.DataCacheDuration), TimeSpan.Zero);
        }

        public static void Insert(string key, Object value)
        {
            if (enabled)
                HttpRuntime.Cache.Insert(key, value);
        }

        public static void Insert(string key, Object value, TimeSpan slidingExpiration)
        {
            if (enabled)
                HttpRuntime.Cache.Insert(key, value, null, Cache.NoAbsoluteExpiration, slidingExpiration);
        }

        public static void Insert(string key, Object value, CacheDependency dependencies)
        {
            if (enabled)
                HttpRuntime.Cache.Insert(key, value, dependencies);
        }

        public static void Insert(string key, Object value, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            if (enabled)
                HttpRuntime.Cache.Insert(key, value, null, absoluteExpiration, slidingExpiration);
        }

        public static void Insert(string key, Object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            if (enabled)
                HttpRuntime.Cache.Insert(key, value, dependencies, absoluteExpiration, slidingExpiration);
        }

        public static int Count()
        {
            return HttpRuntime.Cache.Count;
        }

        public static long EffectivePercentagePhysicalMemoryLimit()
        {
            return HttpRuntime.Cache.EffectivePercentagePhysicalMemoryLimit;
        }

        public static long EffectivePrivateBytesLimit()
        {
            return HttpRuntime.Cache.EffectivePrivateBytesLimit;
        }

        public static void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        public static System.Collections.IDictionaryEnumerator GetEnumerator()
        {
            return HttpRuntime.Cache.GetEnumerator();
        }


        public static string CacheKeyCreator(string Category, dynamic Filter)
        {
            string key = Category;

            foreach (var property in Filter.GetType().GetProperties())
            {
                var value = property.GetValue(Filter);
                if (value != null)
                    key += string.Format("_{0}-{1}", property.Name, value);
            }
            return key;
        }
    }
}