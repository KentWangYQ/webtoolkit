using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Message
{
    public interface IMessage
    {
        string Send(string url);
        string Send(string url, Encoding encoding);
        void SendAsync(string url, DownloadStringCompletedEventHandler DownloadStringCompleted);
        void SendAsync(string url, DownloadDataCompletedEventHandler DownloadDataCompleted);
    }
}
