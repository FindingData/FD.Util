using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Crypto
{
    public static class TripleDesHelper
    {
        /// <summary>
        /// Secret key size in bits.
        /// </summary>
        private const int _keySize = 64;

        /// <summary>
        /// Initialization Vector length in bytes.
        /// </summary>
        private const int _ivLength = 8;
        /// <summary>
        /// Padding for rounding byte count before encryption.
        /// </summary>
        /// <remarks>PaddingModes Zeros and None does not work.</remarks>
        private const PaddingMode _paddingMode
            = PaddingMode.PKCS7;


        #region 3des加密

        /// <summary>
        /// 3des 加密
        /// </summary>
        /// <param name="strPlain"></param>
        /// <param name="strKey"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string Encrypt(string strPlain, string strKey, CipherMode mode = CipherMode.ECB)
        {
            var iv = CryptoHelper.GenerateIv(8);

            var cipher = Encrypt(strPlain, Convert.FromBase64String(strKey), iv);

            var encrypted = CryptoHelper.CombineIvData(iv, Convert.FromBase64String(cipher), _ivLength);

            return Convert.ToBase64String(encrypted);

        }

        /// <summary>
        /// 3des 解密
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Decrypt(string strCipher, string strKey, CipherMode mode = CipherMode.ECB)
        {
            var iv = CryptoHelper.GetIv(Convert.FromBase64String(strCipher), _ivLength);

            var encrypted = CryptoHelper.RemoveIv(Convert.FromBase64String(strCipher), _ivLength);

            return Decrypt(Convert.ToBase64String(encrypted), Convert.FromBase64String(strKey), iv, mode);
        }


        /// <summary>
        /// 3des 加密
        /// </summary>
        /// <param name="strPlain">待加密的字符串</param>
        /// <param name="strKey">密钥</param>
        /// <param name="strIV">加密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="mode">运算模式</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt(string strPlain, byte[] key, byte[] iv, CipherMode mode = CipherMode.ECB)
        {

            byte[] encrypted;
            using (var des = new TripleDESCryptoServiceProvider())
            {
                des.Key = key;
                des.Mode = mode;

                if (mode == CipherMode.CBC)
                {
                    des.IV = iv;
                }

                des.Padding = _paddingMode;
                // Convert to bytes
                byte[] textBytes = Encoding.UTF8.GetBytes(strPlain);

                using (var ms = new MemoryStream())
                using (var encryptor = des.CreateEncryptor())
                {
                    using (var cryptoStream = new CryptoStream(ms, encryptor,
                        CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(textBytes, 0, textBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                    encrypted = ms.ToArray();

                }
            }
            return Convert.ToBase64String(encrypted); 
        }


        /// <summary>
        /// 3des 解密
        /// </summary>
        /// <param name="strCipher">加密的字符串</param>
        /// <param name="strKey">密钥</param>
        /// <param name="strIV">解密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="mode">运算模式</param>
        /// <returns>解密的字符串</returns>
        public static string Decrypt(string strCipher, byte[] key, byte[] iv, CipherMode mode = CipherMode.ECB)
        {

            byte[] decrypted;
            using (var des = new TripleDESCryptoServiceProvider())
            {
                des.Key = key;
                des.Mode = mode;

                if (mode == CipherMode.CBC)
                {
                    des.IV = iv;
                }
                
                             
                des.Padding = _paddingMode;
                // Convert to bytes
                byte[] textBytes = Convert.FromBase64String(strCipher);
                using (var ms = new MemoryStream())
                using (var decryptor = des.CreateDecryptor())
                {
                    using (var cryptoStream = new CryptoStream(ms, decryptor,
                        CryptoStreamMode.Write))
                    {

                        cryptoStream.Write(textBytes, 0,
                            textBytes.Length);
                        cryptoStream.FlushFinalBlock();
                    }
                    decrypted = ms.ToArray();
                }

            }
            return Encoding.UTF8.GetString(decrypted);
        }
        #endregion
    }
}
