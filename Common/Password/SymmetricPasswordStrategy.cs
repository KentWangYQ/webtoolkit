using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Diagnostics;

namespace Common
{
    public class SymmetricPasswordStrategy : PasswordStrategyBase, ISymmetricPasswordStrategy
    {
        #region Fields
        private readonly byte[] Keys = { 0xEF, 0xAB, 0x56, 0x78, 0x90, 0x34, 0xCD, 0x12 };
        private readonly string DefaultCodeKey = "EICEDUCATION";
        #endregion

        #region Properties
        public string CodeKey { get; set; }
        #endregion

        #region .ctor
        /// <summary>
        /// Default .ctor for use Encrypt() only
        /// </summary>
        public SymmetricPasswordStrategy()
            : base(8, PasswordScore.VeryStrong)
        {
            CodeKey = DefaultCodeKey;
        }

        /// <summary>
        /// Use default validation rule
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="score"></param>
        public SymmetricPasswordStrategy(int minLength, PasswordScore score)
            : base(minLength, null, score)
        {
            CodeKey = DefaultCodeKey;
        }

        /// <summary>
        /// Use customized validation rule
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="validationRule"></param>
        /// <param name="score"></param>
        public SymmetricPasswordStrategy(int minLength, List<KeyValuePair<string, string>> validateionRuleDescriptionPair, PasswordScore score)
            : base(minLength, validateionRuleDescriptionPair, score)
        {
            CodeKey = DefaultCodeKey;
        }
        #endregion

        #region Encrypt and Decrypt
        /// <summary>
        /// Password encrypt
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public override string Encrypt(string password)
        {
            var result = password;
            try
            {
                var rgbKey = Encoding.UTF8.GetBytes(CodeKey.Substring(0, 8));
                var rgbIV = Keys;
                var inputBytesAttry = Encoding.UTF8.GetBytes(password);
                var desCryptoServiceProvider = new DESCryptoServiceProvider();
                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(inputBytesAttry, 0, inputBytesAttry.Length);
                    cryptoStream.FlushFinalBlock();
                    result = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return result;
        }

        public string Decrypt(string encryptPassword)
        {
            var result = encryptPassword;
            try
            {
                var rgbKey = Encoding.UTF8.GetBytes(CodeKey.Substring(0, 8));
                var rgbIV = Keys;
                var inputBytesAttry = Convert.FromBase64String(encryptPassword);
                var desCryptoServiceProvider = new DESCryptoServiceProvider();
                using (var memoryStream = new MemoryStream())
                using (var cryptoStream = new CryptoStream(memoryStream, desCryptoServiceProvider.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(inputBytesAttry, 0, inputBytesAttry.Length);
                    cryptoStream.FlushFinalBlock();
                    result = Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
            return result;
        }
        #endregion

        public static SymmetricPasswordStrategy Creator
        {
            get { return new SymmetricPasswordStrategy(); }
        }
    }
}
