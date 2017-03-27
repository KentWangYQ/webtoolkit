using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.OpenAuth
{
    internal class OpenAuthClientHelper
    {
        string cacheKey = "OpenAuthClient_State";
        internal string GetState()
        {
            string state = Guid.NewGuid().ToString().Replace("-", "");
            CollectionCache.Insert(cacheKey, state, null, TimeSpan.FromSeconds(300));
            return state;
        }

        internal bool VerifyState(string state)
        {
            var obj = CollectionCache.Get(cacheKey, state);
            if (obj != null)
            {
                CollectionCache.Remove(cacheKey, state);
                return true;
            }
            return false;
        }

        internal static OpenAuthClientHelper Creator
        {
            get { return new OpenAuthClientHelper(); }
        }
    }
}
