using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;

namespace Common
{
    public class PasswordStrategy : PasswordStrategyBase
    {
        #region .ctor
        /// <summary>
        /// Default .ctor for use Encrypt() only
        /// </summary>
        public PasswordStrategy()
            : base(8, PasswordScore.VeryStrong)
        {
        }

        /// <summary>
        /// Use default validation rule
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="score"></param>
        public PasswordStrategy(int minLength, PasswordScore score)
            : base(minLength, null, score)
        {
        }

        /// <summary>
        /// Use customized validation rule
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="validationRule"></param>
        /// <param name="score"></param>
        public PasswordStrategy(int minLength, List<KeyValuePair<string, string>> validateionRuleDescriptionPair, PasswordScore score)
            : base(minLength, validateionRuleDescriptionPair, score)
        {
        }
        #endregion

        #region public method
        /// <summary>
        /// Password encrypt
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public override string Encrypt(string password)
        {
            Byte[] clearBytes = new UnicodeEncoding().GetBytes(password);
            Byte[] hashedBytes = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(clearBytes);

            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }

        public static string Encrypt1(string password)
        {
            return new PasswordStrategy().Encrypt(password);
        }
        #endregion

        //#region ========加密========

        ///// <summary>
        ///// 加密
        ///// </summary>
        ///// <param name="Text"></param>
        ///// <returns></returns>
        //public static string Encrypt(string Text)
        //{
        //    return Encrypt(Text, "EIC_Connect");
        //}
        ///// <summary> 
        ///// 加密数据 
        ///// </summary> 
        ///// <param name="Text"></param> 
        ///// <param name="sKey"></param> 
        ///// <returns></returns> 
        //public static string Encrypt(string Text, string sKey)
        //{
        //    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        //    byte[] inputByteArray;
        //    inputByteArray = Encoding.Default.GetBytes(Text);
        //    des.Key = ASCIIEncoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        //    des.IV = ASCIIEncoding.ASCII.GetBytes(FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream();
        //    CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
        //    cs.Write(inputByteArray, 0, inputByteArray.Length);
        //    cs.FlushFinalBlock();
        //    StringBuilder ret = new StringBuilder();
        //    foreach (byte b in ms.ToArray())
        //    {
        //        ret.AppendFormat("{0:X2}", b);
        //    }
        //    return ret.ToString();
        //}

        //#endregion

        #region ========解密========


        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, "EIC_Connect");
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            int len;
            len = Text.Length / 2;
            byte[] inputByteArray = new byte[len];
            int x, i;
            for (x = 0; x < len; x++)
            {
                i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            return Encoding.Default.GetString(ms.ToArray());
        }

        #endregion
    }
}
