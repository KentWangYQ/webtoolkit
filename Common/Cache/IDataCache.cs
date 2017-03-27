using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace Common
{
    public interface IDataCache
    {
        Object Get(string key);
        void Insert(string key, Object value);
        void Insert(string key, Object value, CacheDependency dependencies);
        void Insert(string key, Object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration);
        void Insert(string key, Object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority);
        void Insert(string key, Object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback);
        void Add(string key, Object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback);
    }
}
