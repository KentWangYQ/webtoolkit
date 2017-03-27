using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Frequency
{
    public class FrequencyControlSms : FrequencyControlBase
    {
        const string CACHEKEY = "FrequencyControl_Sms_8B2C5E119B8E4742B0F2C430A83C3921";

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

        private static FrequencyControlSms creator = new FrequencyControlSms();
        public static FrequencyControlSms Creator { get { return creator; } }
    }
}
