using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace FD.Util.Crypto
{
    public class HashHelper
    {

        /// <summary>
        /// MD5字符串+盐
        /// </summary>
        /// <param name="plain">字符串</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static string Md5WithSalt(string plain,byte[] salt)
        {
            return Md5(Encoding.UTF8.GetBytes(plain).Concat(salt).ToArray());
        }

        /// <summary>
        /// MD5字符串+盐
        /// </summary>
        /// <param name="plain">字符串</param>
        /// <param name="salt">盐</param>
        /// <returns></returns>
        public static string Md5WithSalt(byte[] plain, byte[] salt)
        {
            return Md5(plain.Concat(salt).ToArray());
        }


        /// <summary>
        /// MD5字符串
        /// </summary>
        /// <param name="plain">字符串</param>
        /// <returns></returns>
        public static string Md5(string plain)
        {
            return Md5(Encoding.UTF8.GetBytes(plain));
        }

        /// <summary>
        /// MD5字节
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns></returns>
        public static string Md5(byte[] bytes)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] retVal = md5.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// MD5文件
        /// </summary>       
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string Md5File(string filePath)
        {
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] retVal = md5.ComputeHash(fs);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < retVal.Length; i++)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                    return sb.ToString();
                }
            }
        }
     
        /// <summary>
        /// Cals the sha1.
        /// </summary>
        /// <see cref="http://www.teimouri.net/calculate-file-checksum-in-c/"/>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
        public static string CalSha1(byte[] bytes)
        {
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] retVal = sha1.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString().ToLowerInvariant();
            }
        }        
    }




  
}
