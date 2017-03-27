using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface ITimeService
    {
        /// <summary>
        /// Get the local time of website hosting server.
        /// </summary>
        /// <remarks>
        /// Server may be hosted by vision or by client, 
        /// ServerLocalTime equals to ClientLocalTime only when
        /// the site is hosted by client.</remarks>
        /// <returns></returns>
        DateTime GetServerTime();

        /// <summary>
        /// Get the local time of client.
        /// </summary>
        /// <returns></returns>
        DateTime GetClientTime();

        /// <summary>
        /// Get UTC time.
        /// </summary>
        /// <returns></returns>
        DateTime GetUTCTime();

        /// <summary>
        /// Get the min datetime of Database
        /// </summary>
        /// <returns></returns>
        DateTime GetDBMinTime();
    }
}
