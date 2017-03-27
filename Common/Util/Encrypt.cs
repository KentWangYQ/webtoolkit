using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Util
{
    public class Encrypt
    {
        public static string MD5(string content)
        {
            byte[] result = Encoding.Default.GetBytes(content);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "");
        }

        public static string MD5ForPHP(string stringToHash)
        {
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] emailBytes = Encoding.UTF8.GetBytes(stringToHash);
            byte[] hashedEmailBytes = md5.ComputeHash(emailBytes);
            StringBuilder sb = new StringBuilder();
            foreach (var b in hashedEmailBytes)
            {
                sb.Append(b.ToString("x2").ToLower());
            }
            return sb.ToString();
        }
    }
}
