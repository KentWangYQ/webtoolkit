using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Util
{
    public class TxtUtility
    {
        public enum LogType
        {
            Message,
            Wraning,
            Error
        }
        static readonly string path = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Log\");
        private static readonly object writeLocker = new object();
        public static void TxtWrite(string text, LogType type = LogType.Message, string logFileName = "")
        {
            try
            {
                lock (writeLocker)
                {
                    string fileName = "Log_" + logFileName + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    FileStream fs = new FileStream(path + fileName, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(DateTime.Now + ": " + "<" + type.ToString() + "> " + text + "\r\n");
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
            }
            catch (Exception ex) { }
        }
    }
}
