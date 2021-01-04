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
        /// RSA的容器 可以解密的源字符串长度为 DWKEYSIZE/8-11 
        /// </summary>
        public const int DWKEYSIZE = 1024;


        /// <summary>
        /// 字符串加密
        /// </summary>
        /// <param name="strPlain">源字符串 明文</param>
        /// <param name="publicKey">公匙或私匙</param>
        /// <returns>加密遇到错误将会返回原字符串</returns>
        public static string Encrypt(string strPlain, string publicKey)
        {

            using (var rsa = new RSACryptoServiceProvider(DWKEYSIZE))
            {                                   
                byte[] textBytes = Encoding.UTF8.GetBytes(strPlain);
                rsa.FromXmlString(publicKey);
                
                byte[] encrypted = rsa.Encrypt(textBytes, false);
                return Convert.ToBase64String(encrypted);
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
            using (var rsa = new RSACryptoServiceProvider(DWKEYSIZE))
            {
                byte[] textBytes = Convert.FromBase64String(strCipher);
                rsa.FromXmlString(privateKey);
                byte[] decrypted = rsa.Decrypt(textBytes, false);
                return Encoding.UTF8.GetString(decrypted);
            }

        }
    }
}