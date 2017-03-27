using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;

namespace Common.Frequency
{
    public abstract class FrequencyControlBase
    {
        public abstract string CacheKey { get; }

        public virtual bool IsOverFrequency(string key, object value, DateTime absoluteExpiration, int frequency)
        { return IsOverFrequency(key, value, absoluteExpiration, Cache.NoSlidingExpiration, frequency); }

        public virtual bool IsOverFrequency(string key, object value, TimeSpan slidingExpiration, int frequency)
        { return IsOverFrequency(key, value, Cache.NoAbsoluteExpiration, slidingExpiration, frequency); }

        public abstract bool IsOverFrequency(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration, int frequency);
    }
}
