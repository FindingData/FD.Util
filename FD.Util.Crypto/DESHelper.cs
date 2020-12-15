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
        /// DES加密的密钥结构
        /// </summary>
        public struct DESKey
        {
            public string Key { get; set; }

            public string IV { get; set; }
        }



        public static DESKey GetDesKey()
        {
            DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
            return new DESKey()
            {
                Key = Encoding.ASCII.GetString(desProvider.Key),
                IV = Encoding.ASCII.GetString(desProvider.IV),
            };
        }

        public static DESKey Get3DesKey()
        {
            TripleDESCryptoServiceProvider desProvider = new TripleDESCryptoServiceProvider();
            return new DESKey()
            {
                Key = Encoding.ASCII.GetString(desProvider.Key),
                IV = Encoding.ASCII.GetString(desProvider.IV),
            };
        }



        public static string Encrypt(string strPlain,string strKey)
        {
            return Encrypt(strPlain, strKey);
        }

        /// <summary>
        /// 解密原函数
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Decrypt(string strCipher, string strKey)
        {
            return Decrypt(strCipher, strKey, strKey);
        }


        /// <summary>
        /// 加密原函数
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Encrypt(string strPlain, string strKey, string strIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();            
            byte[] inputByteArray = Encoding.Default.GetBytes(strPlain);
            des.Key = ASCIIEncoding.Default.GetBytes(strKey);
            des.IV = ASCIIEncoding.Default.GetBytes(strIV);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
            //return a;
        }

        /// <summary>
        /// 解密原函数
        /// </summary>
        /// <param name="strCipher"></param>
        /// <param name="strKey"></param>
        /// <param name="strIV"></param>
        /// <returns></returns>
        public static string Decrypt(string strCipher, string strKey, string strIV)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] inputByteArray = new byte[strCipher.Length / 2];
            for (int x = 0; x < strCipher.Length / 2; x++)
            {
                int i = (Convert.ToInt32(strCipher.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            des.Key = ASCIIEncoding.Default.GetBytes(strKey);
            des.IV = ASCIIEncoding.Default.GetBytes(strIV);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            return System.Text.Encoding.Default.GetString(ms.ToArray());
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
