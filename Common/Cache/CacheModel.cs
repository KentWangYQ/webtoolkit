using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;

namespace Common
{
    internal class CacheModel
    {
        public string Key { get; set; }
        public object Value { get; set; }
        public DateTime InsertTime { get; set; }
        public DateTime AbsoluteExpiration { get; set; }
        public TimeSpan SlidingExpiration { get; set; }

        /// <summary>
        /// 超过absoluteExpiration 或 slidingExpiration即为过期
        /// </summary>
        public bool IsExpired
        {
            get
            {
                return !((this.AbsoluteExpiration == Cache.NoAbsoluteExpiration || DateTime.UtcNow < this.AbsoluteExpiration)
                        && (this.SlidingExpiration == Cache.NoSlidingExpiration || DateTime.UtcNow < this.InsertTime.Add(this.SlidingExpiration)));
            }
        }
    }
}