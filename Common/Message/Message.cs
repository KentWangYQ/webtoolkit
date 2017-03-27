using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Common.Message
{
    public class Message : IMessage
    {
        public string Send(string url)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.Default;
                return client.DownloadString(url);
            }
        }

        public string Send(string url, Encoding encoding)
        {
            using (WebClient client = new WebClient())
            {
                client.Encoding = encoding;
                return client.DownloadString(url);
            }
        }

        public void SendAsync(string url, DownloadStringCompletedEventHandler DownloadStringCompleted = null)
        {
            using (WebClient client = new WebClient())
            {
                if (DownloadStringCompleted != null)
                    client.DownloadStringCompleted += DownloadStringCompleted;
                client.DownloadStringAsync(new Uri(url), url);
            }
        }

        public void SendAsync(string url, DownloadDataCompletedEventHandler DownloadDataCompleted = null)
        {
            using (WebClient client = new WebClient())
            {
                if (DownloadDataCompleted != null)
                    client.DownloadDataCompleted += DownloadDataCompleted;
                client.DownloadDataAsync(new Uri(url), url);
            }
        }
    }
}
