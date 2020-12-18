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
        /// Initialization Vector length in bytes.
        /// </summary>
        private const int _ivLength = 8;
        /// <summary>
        /// Padding for rounding byte count before encryption.
        /// </summary>
        /// <remarks>PaddingModes Zeros and None does not work.</remarks>
        private const PaddingMode _paddingMode
            = PaddingMode.PKCS7;



        /// <summary>
        /// Des加密函数
        /// </summary>
        /// <param name="strPlain"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string Encrypt(string strPlain,string strKey)
        {             
            var iv = CryptoHelper.GenerateIv(8);
            
            var cipher = Encrypt(strPlain, Convert.FromBase64String(strKey), iv);

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

            return Decrypt(Convert.ToBase64String(encrypted), Convert.FromBase64String(strKey), iv);
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
    }
}
