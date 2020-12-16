using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FD.Util.Crypto
{
    public static class AESHelper
    {

        /// <summary>
        /// Secret key size in bits.
        /// </summary>
        private const int _keySize = 256;
        /// <summary>
        /// Initialization Vector length in bytes.
        /// </summary>
        private const int _ivLength = 16;
        /// <summary>
        /// Padding for rounding byte count before encryption.
        /// </summary>
        /// <remarks>PaddingModes Zeros and None does not work.</remarks>
        private const PaddingMode _paddingMode = PaddingMode.PKCS7;


        /// <summary>
        /// Encrypts utf-8 text to get base64 string of format [IV]-[DATA].
        /// </summary>
        /// <param name="strPlain"></param>
        /// <param name="strKey"></param>
        /// <returns></returns>
        public static string Encrypt(string strPlain, string strKey)
        {
            var iv = CryptoHelper.GenerateIv(_ivLength);

            var cipher = Encrypt(strPlain, Encoding.UTF8.GetBytes(strKey), iv);

            var encrypted = CryptoHelper.CombineIvData(iv, Convert.FromBase64String(cipher), _ivLength);

            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        ///  
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


        public static string Encrypt(string strPlain,byte[] key,byte[] iv)
        {
            byte[] encrypted;
            using (var aes = new AesManaged())
            {
                aes.Padding = _paddingMode;
                aes.KeySize = _keySize;

                aes.Key = key;
                aes.IV = iv;
                using (var ms = new MemoryStream())
                using (var encryptor = aes.CreateEncryptor())
                {
                    using (var cryptoStream = new CryptoStream(ms, encryptor,
                        CryptoStreamMode.Write))
                    {
                        // Convert to bytes
                        byte[] textBytes = Encoding.UTF8.GetBytes(strPlain);
                        cryptoStream.Write(textBytes, 0, textBytes.Length);
                    }
                    encrypted = ms.ToArray();                   
                }
            }
            return Convert.ToBase64String(encrypted); ;
        }

        public static string Decrypt(string strCipher, byte[] key, byte[] iv)
        {
            byte[] decrypted;
            using (var aes = new AesManaged())
            {
                aes.Key = key;
                aes.IV = iv;

                aes.Padding = _paddingMode;
                aes.KeySize = _keySize;
                // Convert to bytes
                byte[] textBytes = Convert.FromBase64String(strCipher);
                
                using (var ms = new MemoryStream())
                using (var decryptor = aes.CreateDecryptor())
                {
                    using (var cryptoStream = new CryptoStream(ms, decryptor,
                        CryptoStreamMode.Write))
                    {
                       
                        cryptoStream.Write(textBytes, 0,
                            textBytes.Length);
                    }
                    decrypted = ms.ToArray();
                }
            }       
            return Encoding.UTF8.GetString(decrypted); ;
        }
    }
}
