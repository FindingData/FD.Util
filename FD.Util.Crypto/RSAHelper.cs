using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace FD.Util.Crypto
{
    /// <summary>
    /// 非对称RSA加密类  
    /// 若是私匙加密 则需公钥解密
    /// 反正公钥加密 私匙来解密
    /// 需要BigInteger类来辅助
    /// </summary>
    public static class RSAHelper
    {

        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="strPlain">源字符串 明文</param>
        /// <param name="publicKey">公匙或私匙</param>
        /// <returns>加密遇到错误将会返回原字符串</returns>
        public static string Encrypt(string strPlain, string publicKey)
        {            
            try
            {
                
                using (var rsa = new RSACryptoServiceProvider())
                {
                    byte[] textBytes = Encoding.UTF8.GetBytes(strPlain);
                    rsa.FromXmlString(publicKey);
                    byte[] encrypted = rsa.Decrypt(textBytes, false);
                    return Convert.ToBase64String(encrypted);
                }                                                            
            }
            catch {
                return null;
            }           
        }

        /// <summary>
        /// 字符串解密
        /// </summary>
        /// <param name="strCipher">密文</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>遇到解密失败将会返回原字符串</returns>
        public static string Decrypt(string strCipher, string privateKey)
        {             
            try
            {
                byte[] textBytes = Convert.FromBase64String(strCipher);
                RSACryptoServiceProvider.UseMachineKeyStore = true;
                RSACryptoServiceProvider rsaProvider = new RSACryptoServiceProvider();
                rsaProvider.FromXmlString(privateKey);
                byte[] decrypted = rsaProvider.Decrypt(textBytes, false);
                return Encoding.UTF8.GetString(decrypted);
            }
            catch
            {              
                return null;
            }
         
        }

    }
}