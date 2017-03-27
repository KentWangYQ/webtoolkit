using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Message
{
    public interface ISMS
    {
        string Send(string mobile, string content);
        string Send(string mobile, string content, Encoding encoding);
        void SendAsync(string mobile, string content, DownloadStringCompletedEventHandler DownloadStringCompleted);
        void SendAsync(string mobile, string content, DownloadDataCompletedEventHandler DownloadDataCompleted);
        VerifyStatusCode StatusCheck(string result);
    }
}
