using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Frequency
{
    public class FrequencyControlCall : FrequencyControlBase
    {
        const string CACHEKEY = "FrequencyControl_Call_D9BCBD1E19EE48B385FFBF3CD656644C";

        public override string CacheKey
        {
            get { return CACHEKEY; }
        }
        public override bool IsOverFrequency(string key, object value, DateTime absoluteExpiration, TimeSpan slidingExpiration, int frequency)
        {
            var c = CollectionCache.Get(CacheKey, key);
            if (c == null || c.Count() < frequency)
            {
                CollectionCache.Insert(CacheKey, key, value, absoluteExpiration, slidingExpiration);
                return false;
            }

            return true;
        }

        private static FrequencyControlCall creator = new FrequencyControlCall();
        public static FrequencyControlCall Creator { get { return creator; } }
    }
}
