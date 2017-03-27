using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Call
{
    public abstract class CallClientBase
    {
        #region Property
        protected abstract string BaseUrl { get; }
        protected abstract string CallApi { get; }
        #endregion

        #region Abstract Method
        public abstract string GetCallApiUrl();
        public abstract CallStatusCode Call(string apiUrl, dynamic parameters = null);
        public abstract CallStatusCode Call(string apiUrl, Dictionary<string, object> parameters = null);
        #endregion
    }
}
