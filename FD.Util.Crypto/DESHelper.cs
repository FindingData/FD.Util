using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Crypto
{
    public static class DESHelper
    {

        /// <summary>
        /// Secret key size in bits.
        /// </summary>
        private const int _keySize = 64;

        /// <summary>
        /// Secret key size in bits.
        /// </summary>
        private const int _3KeySize = 128;
        /// <summary>
        /// Initialization Vector length in bytes.
        /// </summary>
        private const int _ivLength = 8;
        /// <summary>
        /// Padding for rounding byte count before encryption.
        /// </summary>
        /// <remarks>PaddingModes Zeros and None does not work.</remarks>
        private const PaddingMode _paddingMode = PaddingMode.PKCS7;



        /// <summary>
        /// Des加密函数
        /// </summary>
        /// <param name="strPlain"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string Encrypt(string strPlain,string strKey)
        {
            var iv = CryptoHelper.GenerateIv(8);

            var cipher = Encrypt(strPlain, Encoding.UTF8.GetBytes(strKey), iv);

            var encrypted = CryptoHelper.CombineIvData(iv, Convert.FromBase64String(cipher), _ivLength);

            return Convert.ToBase64String(encrypted);

        }

        /// <summary>
        /// Des解密函数
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Decrypt(string strCipher, string strKey)
        {
            var iv = CryptoHelper.GetIv(Convert.FromBase64String(strCipher), _ivLength);

            var encrypted = CryptoHelper.RemoveIv(Convert.FromBase64String(strCipher), _ivLength);

            return Decrypt(Convert.ToBase64String(encrypted), Encoding.UTF8.GetBytes(strKey), iv);
        }


        /// <summary>
        /// 加密原函数
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Encrypt(string strPlain, byte[] key, byte[] iv)
        {
            byte[] encrypted;            
            using (var des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.IV = iv;

                des.Padding = _paddingMode;
                des.KeySize = _keySize;

                using (var ms = new MemoryStream())
                using (var encryptor = des.CreateEncryptor())
                {
                    using (var cryptoStream = new CryptoStream(ms, encryptor,
                        CryptoStreamMode.Write))
                    {
                        // Convert to bytes
                        byte[] textBytes = Encoding.UTF8.GetBytes(strPlain);
                        cryptoStream.Write(textBytes, 0, textBytes.Length);
                    }
                    //encrypted = ms.ToArray();
                    StringBuilder ret = new StringBuilder();
                    //decrypted = ms.ToArray();
                    foreach (byte b in ms.ToArray())
                    {
                        ret.AppendFormat("{0:X2}", b);
                    }
                    ret.ToString();
                    return ret.ToString();
                }
            }
            //return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// 解密原函数
        /// </summary>
        /// <param name="strCipher"></param>
        /// <param name="strKey"></param>
        /// <param name="strIV"></param>
        /// <returns></returns>
        public static string Decrypt(string strCipher, byte[] key,byte[] iv)
        {
            byte[] decrypted;
            using (var des = new DESCryptoServiceProvider())
            {
                des.Key = key;
                des.IV = iv;
                des.Padding = _paddingMode;
                des.KeySize = _keySize;

                // Convert to bytes
                //byte[] textBytes = Convert.FromBase64String(strCipher);
                byte[] textBytes = new byte[strCipher.Length / 2];
                for (int x = 0; x < strCipher.Length / 2; x++)
                {
                    int i = (Convert.ToInt32(strCipher.Substring(x * 2, 2), 16));
                    textBytes[x] = (byte)i;
                }

                using (var ms = new MemoryStream())
                using (var decryptor = des.CreateDecryptor())
                {
                    using (var cryptoStream = new CryptoStream(ms, decryptor,
                        CryptoStreamMode.Write))
                    {

                        cryptoStream.Write(textBytes, 0,
                            textBytes.Length);
                    }
                    return System.Text.Encoding.Default.GetString(ms.ToArray());
                }
               
            }
           
            //return Encoding.UTF8.GetString(decrypted);                                

        }


        #region 3des加密

        /// <summary>
        /// 3des 加密
        /// </summary>
        /// <param name="strPlain"></param>
        /// <param name="strKey"></param>
        /// <param name="mode"></param>
        /// <returns></returns>
        public static string Encrypt3Des(string strPlain, string strKey, CipherMode mode)
        {
            return Encrypt3Des(strPlain, strKey, strKey.Substring(0,8), mode);
        }

        /// <summary>
        /// 3des 解密
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Decrypt3Des(string strCipher, string strKey, CipherMode mode)
        {
            return Decrypt3Des(strCipher, strKey, strKey.Substring(0,8), mode);
        }


        /// <summary>
        /// 3des 加密
        /// </summary>
        /// <param name="strPlain">待加密的字符串</param>
        /// <param name="strKey">密钥</param>
        /// <param name="strIV">加密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="mode">运算模式</param>
        /// <returns>加密后的字符串</returns>
        public static string Encrypt3Des(string strPlain, string strKey, string strIV, CipherMode mode)
        {
            
                var des = new TripleDESCryptoServiceProvider
                {
                    Key = Encoding.UTF8.GetBytes(strKey),
                    Mode = mode
                };
                if (mode == CipherMode.CBC)
                {
                    des.IV = Encoding.UTF8.GetBytes(strIV);
                }
                var desEncrypt = des.CreateEncryptor();
                byte[] buffer = Encoding.UTF8.GetBytes(strPlain);
                return Convert.ToBase64String(desEncrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            
        }

      
        /// <summary>
        /// 3des 解密
        /// </summary>
        /// <param name="strCipher">加密的字符串</param>
        /// <param name="strKey">密钥</param>
        /// <param name="strIV">解密矢量：只有在CBC解密模式下才适用</param>
        /// <param name="mode">运算模式</param>
        /// <returns>解密的字符串</returns>
        public static string Decrypt3Des(string strCipher, string strKey,   string strIV, CipherMode mode)
        {

            var des = new TripleDESCryptoServiceProvider
            {
                Key = Encoding.UTF8.GetBytes(strKey),
                Mode = mode,
                Padding = PaddingMode.PKCS7
            };
            if (mode == CipherMode.CBC)
            {
                des.IV = Encoding.UTF8.GetBytes(strIV);
            }
            var desDecrypt = des.CreateDecryptor();
            var result = "";
            byte[] buffer = Convert.FromBase64String(strCipher);
            result = Encoding.UTF8.GetString(desDecrypt.TransformFinalBlock(buffer, 0, buffer.Length));
            return result;
        }
        #endregion
    }
}
